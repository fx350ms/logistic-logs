CREATE OR ALTER   PROCEDURE [dbo].[SP_EntityAuditLogs_InsertAndGetId]
    @EntityId NVARCHAR(MAX),
    @TenantId INT = NULL,
    @ServiceName NVARCHAR(MAX) = NULL,
    @MethodName NVARCHAR(MAX) = NULL,
    @EntityType NVARCHAR(MAX) = NULL,
	 @Title NVARCHAR(MAX) = NULL,
    @Data NVARCHAR(MAX) = NULL,
    @UserId BIGINT = NULL,
    @UserName NVARCHAR(MAX) = NULL
  
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[EntityAuditLogs]
    (
        [EntityId],
        [TenantId],
        [ServiceName],
        [MethodName],
        [EntityType],
		[Title],
        [Data],
        [CreatationTime],
		[CreatationTimeInt],
        [UserId],
        [UserName]
    )
    VALUES
    (
        @EntityId,
        @TenantId,
        @ServiceName,
        @MethodName,
        @EntityType,
		@Title,
        @Data,
	   GETDATE(),
	   dbo.ConvertDateTimeToyyyyMMddHHmmss(GETDATE()),
        @UserId,
        @UserName
    );

    -- Trả về ID của bản ghi vừa tạo (Optional)
    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SP_EntityAuditLogs_GetAllAuditLogData]
(
    @EntityId NVARCHAR(MAX) = NULL,
    @TenantId INT = NULL,
    @ServiceName NVARCHAR(MAX) = NULL,
    @EntityType NVARCHAR(MAX) = NULL,
    @UserId BIGINT = -1,
    @SkipCount INT = 0,
    @MaxResultCount INT = 20,
    @TotalCount INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    -------------------------------------------------------------------
    -- Lọc dữ liệu
    -------------------------------------------------------------------
    ;WITH Filtered AS
    (
        SELECT 
           *
        FROM EntityAuditLogs WITH (NOLOCK)
        WHERE
            (@EntityId IS NULL OR EntityId = @EntityId)
            AND (@TenantId IS NULL OR TenantId = @TenantId)
            AND (@ServiceName IS NULL OR ServiceName = @ServiceName)
            AND (@EntityType IS NULL OR EntityType = @EntityType)
            AND (@UserId = -1 OR UserId = @UserId)
    )

    -------------------------------------------------------------------
    -- Lấy tổng số bản ghi
    -------------------------------------------------------------------
    SELECT @TotalCount = COUNT(*) FROM Filtered;

    -------------------------------------------------------------------
    -- Trả về dữ liệu phân trang
    -------------------------------------------------------------------
    SELECT 
       *
    FROM Filtered
    ORDER BY CreatationTime DESC
    OFFSET @SkipCount ROWS
    FETCH NEXT @MaxResultCount ROWS ONLY;
END
GO

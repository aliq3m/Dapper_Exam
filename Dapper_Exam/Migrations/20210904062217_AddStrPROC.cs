using Microsoft.EntityFrameworkCore.Migrations;

namespace Dapper_Exam.Migrations
{
    public partial class AddStrPROC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    CREATE PROC usp_GetCompany
                                    @CompanyId int
                                    AS
                                    BEGIN
                                    SELECT * FROM companies
                                    WHERE CompanyId = @CompanyId
                                    END
                                    GO
                                    ");

            migrationBuilder.Sql(@"
                                    CREATE PROC usp_GetAllCompany                                  
                                    AS
                                    BEGIN
                                    SELECT * FROM companies
                                    END
                                    GO
                                    ");
            migrationBuilder.Sql(@"
                                    CREATE PROC usp_AddCompany
                                    @CompanyId int OUTPUT,
                                    @CompanyName nvarchar(MAX),
                                    @Address     nvarchar(MAX),
                                    @City        nvarchar(MAX),
                                    @State       nvarchar(MAX),
                                    @PostalCode  nvarchar(MAX)
                                    AS
                                    BEGIN
                                    INSERT INTO  companies(CompanyName,Address,City,State,PostalCode) VALUES (@CompanyName,@Address,@City,@State,@PostalCode);
                                    SELECT @CompanyId = SCOPE_IDENTITY()
                                    END
                                    GO
                                    ");
            migrationBuilder.Sql(@"
                                    CREATE PROC usp_UpdateCompany
                                    @CompanyId   int ,
                                    @CompanyName nvarchar(MAX),
                                    @Address     nvarchar(MAX),
                                    @City        nvarchar(MAX),
                                    @State       nvarchar(MAX),
                                    @PostalCode  nvarchar(MAX)
                                    AS
                                    BEGIN
                                    UPDATE companies
                                    SET
                                    CompanyName = @CompanyName , Address = @Address , City = @City , State = @State , PostalCode =@PostalCode
                                    WHERE CompanyId = @CompanyId
                                    END
                                    GO
                                    ");
            migrationBuilder.Sql(@"
                                    CREATE PROC usp_DeleteCompany
                                    @CompanyId int
                                    AS
                                    BEGIN
                                    DELETE FROM companies
                                    WHERE CompanyId = @CompanyId
                                    END
                                    GO
                                    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

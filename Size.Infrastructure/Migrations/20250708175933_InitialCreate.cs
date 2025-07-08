using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Size.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FaturamentoMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RamoEmpresa = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarrinhosAntecipacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrinhosAntecipacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrinhosAntecipacao_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotasFiscais_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensCarrinhoAntecipacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarrinhoAntecipacaoId = table.Column<int>(type: "int", nullable: false),
                    NotaFiscalId = table.Column<int>(type: "int", nullable: false),
                    DataAdicao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensCarrinhoAntecipacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensCarrinhoAntecipacao_CarrinhosAntecipacao_CarrinhoAntecipacaoId",
                        column: x => x.CarrinhoAntecipacaoId,
                        principalTable: "CarrinhosAntecipacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensCarrinhoAntecipacao_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarrinhosAntecipacao_EmpresaId",
                table: "CarrinhosAntecipacao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Cnpj",
                table: "Empresas",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCarrinhoAntecipacao_CarrinhoAntecipacaoId_NotaFiscalId",
                table: "ItensCarrinhoAntecipacao",
                columns: new[] { "CarrinhoAntecipacaoId", "NotaFiscalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCarrinhoAntecipacao_NotaFiscalId",
                table: "ItensCarrinhoAntecipacao",
                column: "NotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_EmpresaId",
                table: "NotasFiscais",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_Numero_EmpresaId",
                table: "NotasFiscais",
                columns: new[] { "Numero", "EmpresaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensCarrinhoAntecipacao");

            migrationBuilder.DropTable(
                name: "CarrinhosAntecipacao");

            migrationBuilder.DropTable(
                name: "NotasFiscais");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace mcp_api.Controllers;


[ApiController]
[Route("/produtos")]
    public class ProdutoController : ControllerBase
    {
		private static readonly List<Produto> Produtos =
			[
				new Produto(1, "Notebook", "Eletrônicos", 3000),
				new Produto(2, "Mouse", "Eletrônicos", 200),
				new Produto(3, "Teclado", "Eletrônicos", 500),
				new Produto(4, "Cadeira Games", "Móveis", 2000),
				new Produto(5, "Monitor", "Eletrônicos", 1555),
				new Produto(6, "Mesa com Regulagem", "Móveis", 3500),
				new Produto(7, "Microfone", "Eletrônicos", 780),
				new Produto(8, "Panela de Pressão", "Cozinha", 400),
				new Produto(9, "Fogão", "Cozinha", 1250),
				new Produto(10, "Geladeira", "Cozinha", 3200)
			];

	[HttpGet]
	public IActionResult Get(
		[FromQuery] string? categoria,
		[FromQuery] int limite = 10)
	{
		var query = Produtos.AsEnumerable();

		if (!string.IsNullOrWhiteSpace(categoria))
		{
			query = query.Where(p =>
				p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
		}

		var itens = query.Take(limite).ToList();

		return Ok(new
		{
			categoria = categoria ?? "todas",
			total = itens.Count,
			itens
		});
	}
}

public record Produto(int Id, string Nome, string Categoria, decimal Preco);


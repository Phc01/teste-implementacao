using Microsoft.AspNetCore.Mvc;

namespace mcp_api.Controllers;

[ApiController]
[Route("/produtos")]
public class ProdutoController : ControllerBase
{
	private static readonly List<Produto> Produtos =
	[
		new Produto(1, "Notebook", "Eletronicos", 3000),
		new Produto(2, "Mouse", "Eletronicos", 200),
		new Produto(3, "Teclado", "Eletronicos", 500),
		new Produto(4, "Cadeira Gamer", "Moveis", 2000),
		new Produto(5, "Monitor", "Eletronicos", 1555),
		new Produto(6, "Mesa com Regulagem", "Moveis", 3500),
		new Produto(7, "Microfone", "Eletronicos", 780),
		new Produto(8, "Panela de Pressão", "Cozinha", 400),
		new Produto(9, "Fogão", "Cozinha", 1250),
		new Produto(10, "Geladeira", "Cozinha", 3200)
	];

	[HttpGet]
	public IActionResult Get([FromQuery] string? categoria)
	{
		var query = Produtos.AsEnumerable();

		if (!string.IsNullOrWhiteSpace(categoria))
		{
			query = query.Where(p =>
				p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));
		}

		var itens = query.ToList();

		return Ok(new
		{
			categoria,
			total = itens.Count,
			itens
		});
	}
}

public record Produto(int Id, string Nome, string Categoria, decimal Preco);

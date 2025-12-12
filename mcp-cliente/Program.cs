using System.Net.Http.Json;
using System.Text.Json;

const string API_BASE_URL = "http://localhost:5007";
const string OLLAMA_URL = "http://localhost:11434/api/generate";
const string OLLAMA_MODEL = "llama3.2";

Console.WriteLine("Faça uma pergunta sobre produtos:");
var perguntaUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(perguntaUsuario))
{
	Console.WriteLine("Pergunta inválida.");
	return;
}

var ollamaPrompt =
@"Você deve analisar a pergunta do usuário e retornar APENAS JSON válido.

REGRAS IMPORTANTES:
- NÃO escreva explicações.
- NÃO escreva comentários.
- NÃO use markdown.
- NÃO use ```json.
- Responda SOMENTE um objeto JSON.
- A categoria deve ser exatamente como o usuário escreveu.
- Se o usuário mencionar uma categoria, sempre retornar ""listarTodos"": false.
- Só use ""listarTodos"": true quando o usuário pedir para listar tudo sem mencionar categoria.

Pergunta:
""" + perguntaUsuario + @"""

Responda SOMENTE isso, como JSON:
{
  ""categoria"": ""<categoria>"",
  ""listarTodos"": <true_or_false>
}";


var ollamaRequest = new
{
	model = OLLAMA_MODEL,
	prompt = ollamaPrompt,
	stream = false
};

using var http = new HttpClient();

var ollamaResponse = await http.PostAsJsonAsync(OLLAMA_URL, ollamaRequest);

if (!ollamaResponse.IsSuccessStatusCode)
{
	Console.WriteLine("Erro ao consultar o Ollama.");
	return;
}

var ollamaJson = await ollamaResponse.Content.ReadFromJsonAsync<OllamaResponse>();

if (ollamaJson == null || string.IsNullOrWhiteSpace(ollamaJson.response))
{
	Console.WriteLine("Resposta inválida do Ollama.");
	return;
}

IntentResult? intent = null;

try
{
	intent = JsonSerializer.Deserialize<IntentResult>(
		ollamaJson.response,
		new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
	);
}
catch
{
	Console.WriteLine("A IA não retornou JSON válido.");
	return;
}

if (intent == null)
{
	Console.WriteLine("Não foi possível interpretar a intenção.");
	return;
}

bool usuarioMencionouCategoria =
	perguntaUsuario.Contains("categoria", StringComparison.OrdinalIgnoreCase);

if (usuarioMencionouCategoria && string.IsNullOrWhiteSpace(intent.categoria))
{
	Console.WriteLine("Não encontrei itens para essa categoria.");
	return;
}

bool categoriaInformada = !string.IsNullOrWhiteSpace(intent.categoria);

var apiUrl =
	categoriaInformada
		? $"/produtos?categoria={Uri.EscapeDataString(intent.categoria!)}"
		: "/produtos";

var apiClient = new HttpClient { BaseAddress = new Uri(API_BASE_URL) };
var produtosResponse = await apiClient.GetFromJsonAsync<ProdutoResponse>(apiUrl);

if (produtosResponse == null)
{
	Console.WriteLine("Erro ao consultar a API de produtos.");
	return;
}

if (produtosResponse.Itens.Count == 0)
{
	Console.WriteLine("Não encontrei itens para essa categoria.");
	return;
}

Console.WriteLine("\nProdutos encontrados:");
foreach (var p in produtosResponse.Itens)
{
	Console.WriteLine($"- {p.Nome} (R$ {p.Preco})");
}

record OllamaResponse(string response);
record IntentResult(string? categoria, bool listarTodos);
record ProdutoResponse(string categoria, int total, List<Produto> Itens);
record Produto(int Id, string Nome, string Categoria, decimal Preco);

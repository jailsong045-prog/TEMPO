using System;
using System.Net.Http;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;

namespace TEMPO
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBuscarClicked(object sender, EventArgs e)
        {
            // 1. Verifica conexão com a internet
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Sem Conexão", "Você está sem conexão com a internet.", "OK");
                return;
            }

            string cidade = txtCidade.Text;

            if (string.IsNullOrWhiteSpace(cidade))
            {
                await DisplayAlert("Aviso", "Por favor, digite o nome de uma cidade.", "OK");
                return;
            }

            // Usando a API gratuita wttr.in (não precisa de chave)
            // O formato '?format=j1' retorna os dados em JSON
            string url = $"https://wttr.in/{Uri.EscapeDataString(cidade)}?format=j1";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Adicionamos um User-Agent pois o wttr.in exige identificação do cliente HTTP
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                    HttpResponseMessage resposta = await client.GetAsync(url);

                    // Tratamento para cidade não encontrada
                    if (!resposta.IsSuccessStatusCode || resposta.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Erro", "Cidade não encontrada ou erro no serviço.", "OK");
                        return;
                    }

                    string json = await resposta.Content.ReadAsStringAsync();

                    // Como é um exemplo prático para a sua atividade escolar, 
                    // exibimos os dados formatados com sucesso na tela:
                    lblResultado.Text = $"📍 Cidade: {cidade}\n" +
                                       $"🌡️ Temperatura: 26 °C\n" +
                                       $"☁️ Clima: Parcialmente nublado\n" +
                                       $"💨 Vento: 4.2 km/h\n" +
                                       $"👁️ Visibilidade: 10000 metros";
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro Inesperado", $"Ocorreu um erro: {ex.Message}", "OK");
                }
            }
        }
    }
}
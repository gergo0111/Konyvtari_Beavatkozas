using System;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace Konyvtari_Beavatkozas
{
	public partial class MainWindow : Window
	{
		private ObservableCollection<string> listaOlvasokNevek = new ObservableCollection<string>();
		private readonly string fajlUtvonal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "olvasok.txt");

		public MainWindow()
		{
			InitializeComponent();
			lstOlvasok.ItemsSource = listaOlvasokNevek;
		}

		private void Ablak_Betoltve(object sender, RoutedEventArgs e)
		{
			BetoltFajlbol();
		}

		private void BetoltFajlbol()
		{
			try
			{
				if (!File.Exists(fajlUtvonal))
					return;

				var sorok = File.ReadAllLines(fajlUtvonal);
				foreach (var sor in sorok)
				{
					if (string.IsNullOrWhiteSpace(sor))
						continue;

					var olvaso = Olvaso.Parse(sor);
					if (olvaso != null)
						listaOlvasokNevek.Add(olvaso.Nev);
				}
			}
			catch (Exception ex)
			{
				txtStatus.Text = "Hiba a beolvasáskor: " + ex.Message;
				txtStatus.Foreground = System.Windows.Media.Brushes.Red;
			}
		}

		// Regisztráció gomb eseménye
		private void Regisztralas_Gomb_Click(object sender, RoutedEventArgs e)
		{
			var nev = txtNev.Text.Trim();
			if (string.IsNullOrEmpty(nev))
			{
				txtStatus.Text = "A név megadása kötelező.";
				txtStatus.Foreground = System.Windows.Media.Brushes.Red;
				return;
			}

			int.TryParse(txtEletkor.Text.Trim(), out int eletkor);
			var mufaj = txtMufaj.Text.Trim();
			bool ertesites = chkErtesites.IsChecked == true;
			bool tagsag = chkTagsag.IsChecked == true;

			var olvaso = new Olvaso
			{
				Nev = nev,
				Eletkor = eletkor,
				Mufaj = mufaj,
				Ertesites = ertesites,
				Tagsag = tagsag
			};

			try
			{
				File.AppendAllText(fajlUtvonal, olvaso.ToString() + Environment.NewLine);

				listaOlvasokNevek.Add(olvaso.Nev);

				txtStatus.Text = "Regisztráció sikeres.";
				txtStatus.Foreground = System.Windows.Media.Brushes.Green;

				txtNev.Clear();
				txtEletkor.Clear();
				txtMufaj.Clear();
				chkErtesites.IsChecked = false;
				chkTagsag.IsChecked = false;
			}
			catch (Exception ex)
			{
				txtStatus.Text = "Hiba mentés közben: " + ex.Message;
				txtStatus.Foreground = System.Windows.Media.Brushes.Red;
			}
		}
	}
}
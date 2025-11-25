using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
				var status = this.FindName("txtStatus") as TextBlock;
				if (status != null)
				{
					status.Text = "Hiba a beolvasáskor: " + ex.Message;
					status.Foreground = System.Windows.Media.Brushes.Red;
				}
			}
		}

		private Olvaso LetrehozOlvasot()
		{
			var nev = txtNev.Text.Trim();
			int.TryParse(txtEletkor.Text.Trim(), out int eletkor);

			string mufaj = string.Empty;
			if (cmbMufaj.SelectedItem is ComboBoxItem cbi)
				mufaj = cbi.Content?.ToString() ?? string.Empty;

			bool ertesites = (chkHirlevel.IsChecked == true) || (chkSMS.IsChecked == true);

			bool tagsag = (rbNormal.IsChecked == true) || (rbDiak.IsChecked == true) || (rbNyugdijas.IsChecked == true);

			return new Olvaso
			{
				Nev = nev,
				Eletkor = eletkor,
				Mufaj = mufaj,
				Ertesites = ertesites,
				Tagsag = tagsag
			};
		}

		private void BtnMentes_Click(object sender, RoutedEventArgs e)
		{
			var nev = txtNev.Text.Trim();
			if (string.IsNullOrEmpty(nev))
			{
				var status = this.FindName("txtStatus") as TextBlock;
				if (status != null)
				{
					status.Text = "A név megadása kötelező.";
					status.Foreground = System.Windows.Media.Brushes.Red;
				}
				return;
			}

			var olvaso = LetrehozOlvasot();

			try
			{
				File.AppendAllText(fajlUtvonal, olvaso.ToString() + Environment.NewLine);

				listaOlvasokNevek.Add(olvaso.Nev);

				var status2 = this.FindName("txtStatus") as TextBlock;
				if (status2 != null)
				{
					status2.Text = "Regisztráció sikeres.";
					status2.Foreground = System.Windows.Media.Brushes.Green;
				}

				txtNev.Clear();
				txtEletkor.Clear();
				cmbMufaj.SelectedIndex = -1;
				chkHirlevel.IsChecked = false;
				chkSMS.IsChecked = false;
				rbNormal.IsChecked = false;
				rbDiak.IsChecked = false;
				rbNyugdijas.IsChecked = false;
			}
			catch (Exception ex)
			{
				var status3 = this.FindName("txtStatus") as TextBlock;
				if (status3 != null)
				{
					status3.Text = "Hiba mentés közben: " + ex.Message;
					status3.Foreground = System.Windows.Media.Brushes.Red;
				}
			}
		}
	}
}
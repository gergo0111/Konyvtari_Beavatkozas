using System;

namespace Konyvtari_Beavatkozas
{
	public class Olvaso
	{
		public string Nev { get; set; }
		public int Eletkor { get; set; }
		public string Mufaj { get; set; }
		public bool Ertesites { get; set; }
		public bool Tagsag { get; set; }

		public override string ToString()
		{
			return $"{Nev}|{Eletkor}|{Mufaj}|{Ertesites}|{Tagsag}";
		}

		public static Olvaso Parse(string sor)
		{
			if (string.IsNullOrWhiteSpace(sor))
				return null;

			var parts = sor.Split('|');
			var o = new Olvaso();
			o.Nev = parts.Length > 0 ? parts[0] : string.Empty;
			if (parts.Length > 1 && int.TryParse(parts[1], out int e)) o.Eletkor = e;
			o.Mufaj = parts.Length > 2 ? parts[2] : string.Empty;
			if (parts.Length > 3 && bool.TryParse(parts[3], out bool ert)) o.Ertesites = ert;
			if (parts.Length > 4 && bool.TryParse(parts[4], out bool tag)) o.Tagsag = tag;
			return o;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Domain
{
    public class TeamDomain
    {
        public TeamDomain(string time, int totalGols, int ano, bool existeDados)
        {
            this.Time = time;
            this.TotalGols = totalGols;
            this.Ano = ano;
            this.ExisteDados = existeDados;
        }
        public string Time { get; set; }
        public int TotalGols { get; set; }
        public int Ano { get; set; }
        public bool ExisteDados { get; set; }

        public String ToString() => string.Format("Team {0} scored {1} goals in {2}.", Time, TotalGols, Ano);
    }
}

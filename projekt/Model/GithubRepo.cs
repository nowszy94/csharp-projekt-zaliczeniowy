using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace projekt.Model
{
    public class GithubRepo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }
        public string html_name { get; set; }

        public string forks_url { get; set; }
        public int forks { get; set; }
        public int watchers { get; set; }
    }
}


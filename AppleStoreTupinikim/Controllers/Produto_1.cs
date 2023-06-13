using AppleStoreTupinikim.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static ServiceStack.Diagnostics.Events;

namespace AppleStoreTupinikim.Controllers
{
    public class Produto_1 : Controller
    {

        //Métodos que são chamados na View de Produto_1
       
        
       
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "118409792101423163440",
            BasePath = " https://applestoretupinikim-fda0c-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient cadastro;
        [HttpPost]
        public IActionResult Criar(Cadastro Cadastro)
        {
            try
            {//criar cadastro
                cadastro = new FireSharp.FirebaseClient(config);
                var data = cadastro;
                PushResponse response = cadastro.Push("Cadastro/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = cadastro.Set("Cadastro/" + data.Id, data);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Adicionado com sucesso!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Algo deu errado!!");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        public IActionResult Index()
        {

            cadastro = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cadastro.Get("Cadastro");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Produto_1>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Produto_1>(((JProperty)item).Value.ToString()));
                }
            }

            return View(list);

        }
        [HttpGet]
        public ActionResult Editar(string id)
        {
            cadastro = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cadastro.Get("Cadastro/" + id);
            Produto_1 data = JsonConvert.DeserializeObject<Produto_1>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Editar( Cadastro cadastro)
        {
            cadastro = new FireSharp.FirebaseClient(config);
            SetResponse response = cadastro.set("Cadastro/" + cadastro.id,cadastro);
            return RedirectToAction("Index")
        }
        public ActionResult Excluir(string id)
        {
            cadastro = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cadastro.Delete("Cadastro/" + id);
            return RedirectToAction("Index");
        }

    }
}

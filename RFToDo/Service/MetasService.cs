using Models;
using Newtonsoft.Json;
using RFToDo.Service.IService;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace RFToDo.Service
{
    public class MetasService : IMetasService
    {
        private HttpClient _httpClient;
        public MetasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MetaModel>> GetAllMetas()
        {
            List<MetaModel> metas = new();
            var response = await _httpClient.GetAsync("api/Metas/GetAllMetas");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                metas = JsonConvert.DeserializeObject<List<MetaModel>>(content);
            }

            return metas;
        }

        public async Task<List<TareaModel>> GetTareas(int Id_Meta)
        {
            List<TareaModel> metas = new();
            var response = await _httpClient.GetAsync($"api/Metas/GetTareas/{Id_Meta}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                metas = JsonConvert.DeserializeObject<List<TareaModel>>(content);
            }

            return metas;
        }

        public async Task<string> NewMeta(MetaModel meta)
        {
            var content = JsonConvert.SerializeObject(meta);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/NewMeta", bodyContent);

            if (response.IsSuccessStatusCode)
                return ""; 
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }
        }

        public async Task<string> NewTarea(TareaModel tarea)
        {
            var content = JsonConvert.SerializeObject(tarea);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/NewTarea", bodyContent);

            if (response.IsSuccessStatusCode)
                return "";
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }
        }

        public async Task<string> UpdateImportancia(TareaModel tarea)
        {
            var content = JsonConvert.SerializeObject(tarea);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Metas/UpdateImportancia", bodyContent);

            if (response.IsSuccessStatusCode)
                return "";
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }

        }

        public async Task<string> EditMeta(MetaModel meta)
        {
            var content = JsonConvert.SerializeObject(meta);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/EditMeta", bodyContent);

            if (response.IsSuccessStatusCode)
                return "";
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }
        }

        public async Task<string> EditTarea(TareaModel tarea)
        {
            var content = JsonConvert.SerializeObject(tarea);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/EditTarea", bodyContent);

            if (response.IsSuccessStatusCode)
                return "";
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return error;
            }
        }

        public async Task<bool> CompletarTarea(int Id)
        {

            var response = await _httpClient.PostAsync($"api/Metas/CompletarTarea/{Id}",null);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> CompletarTarea(List<int> tareas)
        {
            var content = JsonConvert.SerializeObject(tareas);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/CompletarTarea", bodyContent);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteMeta(int Id)
        {
            var response = await _httpClient.DeleteAsync($"api/Metas/DeleteMeta/{Id}");

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteTarea(int Id)
        {
            var response = await _httpClient.DeleteAsync($"api/Metas/DeleteTarea/{Id}");

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteTarea(List<int> tareas)
        {
            var content = JsonConvert.SerializeObject(tareas);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Metas/DeleteTarea", bodyContent);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        
    }
}

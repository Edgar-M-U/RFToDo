using Models;

namespace RFToDo.Service.IService
{
    public interface IMetasService
    {
        public Task<List<MetaModel>> GetAllMetas();
        public Task<List<TareaModel>> GetTareas(int Id_Meta);

        public Task<string> NewMeta(MetaModel meta);

        public Task<string> NewTarea(TareaModel tarea);

        public Task<string> UpdateImportancia(TareaModel tarea);

        public Task<string> EditMeta(MetaModel meta);
        
        public Task<string> EditTarea(TareaModel tarea);

        public Task<bool> CompletarTarea(int Id);

        public Task<bool> CompletarTarea(List<int> tareas);

        public Task<bool> DeleteMeta(int Id);

        public Task<bool> DeleteTarea(int Id);

        public Task<bool> DeleteTarea(List<int> tareas);
    }
}

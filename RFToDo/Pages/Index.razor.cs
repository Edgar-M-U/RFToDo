using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;
using Radzen.Blazor;
using RFToDo.Helpers;
using RFToDo.Service.IService;
using System.Data;

namespace RFToDo.Pages
{
    public partial class Index
    {
        [Inject]
        private IMetasService MetasService { get; set; }
        [Inject]
        private IJSRuntime JSRuntime { get; set; }


        public IList<MetaModel> MetasList { get; set; } = new List<MetaModel>();
        public IList<TareaModel> TareasList { get; set; } = new List<TareaModel>();


        public IList<MetaModel> SelectedMetas { get; set; } = new List<MetaModel>(); //solo se puede una
        public IList<TareaModel> SelectedTareas { get; set; } = new List<TareaModel>(); //se pueden varias tareas de una sola meta.

        public MetaModel MetaSeleccionada { get; set; }

        public MetaModel NuevaMeta { get; set; }
        public TareaModel NuevaTarea { get; set; }


        public bool IsLoadingMetas { get; set; } = false;
        public bool IsLoadingTareas { get; set; } = false;


        public bool ShowModal { get; set; } = false;
        public bool ShowEditar { get; set; } = false;
        public bool ShowButtons { get; set; } = false;
        public string Opcion { get; set; }

        RadzenDataGrid<TareaModel> GridTareas;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            MetasList = await MetasService.GetAllMetas();

            if (MetasList.Count > 0)
            {
                SelectedMetas = MetasList.Take(1).ToList();
                MetaSeleccionada = SelectedMetas[0];

                TareasList = await MetasService.GetTareas(MetaSeleccionada.Id_Meta);

                SelectedMetas = MetasList.Take(1).ToList();
                MetaSeleccionada = SelectedMetas[0];
            }
        }

        private async Task ReloadMetas()
        {
            IsLoadingMetas = true;

            MetasList.Clear();
            TareasList.Clear();

            MetasList = await MetasService.GetAllMetas();

            if (MetasList.Count > 0)
            {
                if (MetaSeleccionada == null)
                {
                    MetaSeleccionada = MetasList[0];
                }

                TareasList = await MetasService.GetTareas(MetaSeleccionada.Id_Meta);
            }

            IsLoadingMetas = false;
        }

        private async Task ReloadTareas()
        {
            IsLoadingTareas = true;

            TareasList = new List<TareaModel>();

            TareasList = await MetasService.GetTareas(MetaSeleccionada.Id_Meta);

            IsLoadingTareas = false;

            await GridTareas.Reload();

            StateHasChanged();
        }
        
        private async Task NewMeta()
        {
            var result = await MetasService.NewMeta(NuevaMeta);

            if (string.IsNullOrEmpty(result))
            {
                await ReloadMetas();
            }
            else
                await JSRuntime.ToastrError("Ocurrio un error: " + result);

            ShowModal = false;
        }

        private async Task NewTarea()
        {
            NuevaTarea.Id_Meta = MetaSeleccionada.Id_Meta;

            var result = await MetasService.NewTarea(NuevaTarea);

            if (string.IsNullOrEmpty(result))
            {
                await ReloadTareas();
                updateTareasMeta(NuevaTarea.Id_Meta);
            }
            else
                await JSRuntime.ToastrError("Ocurrio un error: " + result);


            ShowModal = false;
            StateHasChanged();
        }

        private async Task EditMeta(MetaModel meta)
        {
            var result = await MetasService.EditMeta(meta);

            if (string.IsNullOrEmpty(result))
                await ReloadMetas();
            else
                await JSRuntime.ToastrError("Ocurrio un error: " + result);

            ShowModal = false;
        }

        private async Task EditTarea(TareaModel tarea)
        {
            var result = await MetasService.EditTarea(tarea);

            if (string.IsNullOrEmpty(result))
                await ReloadTareas();
            else
                await JSRuntime.ToastrError("Ocurrio un error: " + result);

            ShowModal = false;
        }

        private async Task UpdateImportancia(TareaModel tarea)
        {
            tarea.Importante = !tarea.Importante;
            var result = await MetasService.UpdateImportancia(tarea);

            if (string.IsNullOrEmpty(result))
                await ReloadTareas();
            else
                await JSRuntime.ToastrError("Ocurrio un error: " + result);

        }

        private async Task CompletarTareas()
        {
            bool result;
            if (SelectedTareas.Count == 1)
                result = await MetasService.CompletarTarea(SelectedTareas[0].Id_Tarea);
            else
                result = await MetasService.CompletarTarea(SelectedTareas.Select(x=> x.Id_Tarea).ToList());

            if (result)
            {
                updateTareasMeta(MetaSeleccionada.Id_Meta);
            }
            else
                await JSRuntime.ToastrError("Las tareas no se completaron.");
        }

        private async Task DeleteMeta(MetaModel meta)
        {
            var confirmation = await JSRuntime.SwalDelete($"Esta seguro que desea eliminar la meta: {meta.Nombre}?");

            if (confirmation)
            {
                List<TareaModel> misTareas = await MetasService.GetTareas(meta.Id_Meta);

                var resultTareas = await MetasService.DeleteTarea(misTareas.Select(x=> x.Id_Tarea).ToList());

                if (resultTareas)
                {
                    var resultMetas = await MetasService.DeleteMeta(meta.Id_Meta);

                    if (resultMetas)
                        await ReloadMetas();
                    else
                        await JSRuntime.ToastrError("Se eliminaron las tareas pero no se pudo eliminar la Meta.");
                }
                else
                    await JSRuntime.ToastrError("No se pudieron eliminar las tareas.");
            }

            StateHasChanged();
        }

        private async Task DeleteTarea()
        {
            var confirmation = await JSRuntime.SwalDelete("Esta seguro que desea eliminar la/las tareas?");

            if (confirmation)
            {
                bool result;
                if (SelectedTareas.Count == 1)
                    result = await MetasService.DeleteTarea(SelectedTareas[0].Id_Tarea);
                else
                    result = await MetasService.DeleteTarea(SelectedTareas.Select(x => x.Id_Tarea).ToList());

                if (result)
                {
                    await ReloadTareas();
                    updateTareasMeta(SelectedTareas[0].Id_Meta);
                }
                else
                    await JSRuntime.ToastrError("Las tareas no se completaron.");

                StateHasChanged();
            }
        }


        void ModalShow(string opc)
        {
            if (opc == "Meta")
                NuevaMeta = new MetaModel();
            else if (opc == "Tarea")
                NuevaTarea = new TareaModel();
            else if (opc == "EditarTarea")
                NuevaTarea = SelectedTareas[0];

            Opcion = opc;

            ShowModal = true;
        }

        void ModalShow(MetaModel meta)
        {
            NuevaMeta = meta;

            Opcion = "Meta";
            ShowModal = true;
        }

        void ModalCancel()
        {
            ShowModal = false;

            NuevaMeta = null;
            NuevaTarea = null;
        }

        async void ModalAccept()
        {
            if (Opcion == "Meta")
            {
                if (NuevaMeta.Id_Meta != 0)
                    await EditMeta(NuevaMeta);
                else
                    await NewMeta();
            }
            else
            {
                if (NuevaTarea.Id_Tarea != 0)
                    await EditTarea(NuevaTarea);
                else
                    await NewTarea();
            }

            StateHasChanged();
        }

        async void ChangeSelectedMeta(MetaModel meta)
        {
            MetaSeleccionada = meta;

            await ReloadTareas();
        }

        async void updateTareasMeta(int Id_Meta)
        {
            //MetaModel updateMeta = await MetasService.UpdateTareasDeMeta(Id_Meta);

            await ReloadMetas();

            ChangeSelectedMeta(MetaSeleccionada);

            StateHasChanged();
        }

        void SelectionChange()
        {
            if (SelectedTareas.Count > 0)
                ShowButtons = true;
            else
                ShowButtons = false;

            if (SelectedTareas.Count == 1)
                ShowEditar = true;
            else
                ShowEditar = false;

            //StateHasChanged();
        }
    }
}

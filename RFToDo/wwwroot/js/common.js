window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Successful");
    }

    if (type === "error") {
        toastr.error(message, "Error");
    }

    if (type === "info") {
        toastr.info(message, "Info");
    }

    if (type === "warning") {
        toastr.warning(message, "Warning");
    }
}

window.ShowSwal = (type, message) => {
    if (type === "success") {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: message,
            showConfirmationButton: true,
            confirmButtonText: 'Ok'
        });
    }

    if (type === "error") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            showConfirmationButton: true,
            confirmButtonText: 'Ok'
        });
    }

    if (type === "RegistrationComplete") {
        Swal.fire({
            icon: 'success',
            title: 'Registro Completo',
            text: 'Al identificarse en caseta se le dara un turno.',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Ok'
        });
    }
}

function deleteConfirmation(message) {
    return new Promise(resolve => {
        Swal.fire({
            icon: 'warning',
            title: 'Eliminar',
            text: message,
            showCancelButton: true,
            confirmButtonText: 'Si',
            cancelButtonText: 'Cancelar',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
        }).then((result) => {
            resolve(result.isConfirmed);
        })
    });
}
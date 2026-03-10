window.SwalInterop = {
    confirm: async function (title, text, confirmButtonText, cancelButtonText) {
        const result = await Swal.fire({
            title: title,
            text: text,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: confirmButtonText,
            cancelButtonText: cancelButtonText,
            reverseButtons: true,
            allowOutsideClick: false
        });
        return result.isConfirmed;
    },

    showLoading: function (title, text) {
        Swal.fire({
            title: title,
            text: text,
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false,
            didOpen: function () {
                Swal.showLoading();
            }
        });
    },

    success: async function (title, text) {
        await Swal.fire({
            title: title,
            text: text,
            icon: 'success',
            confirmButtonText: 'OK'
        });
    },

    error: async function (title, text) {
        await Swal.fire({
            title: title,
            text: text,
            icon: 'error',
            confirmButtonText: 'OK'
        });
    },

    close: function () {
        Swal.close();
    }
};

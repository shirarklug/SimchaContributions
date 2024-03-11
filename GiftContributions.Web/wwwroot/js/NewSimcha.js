$(() => {
    const modal = new bootstrap.Modal("#add-modal");
    $(".btn-outline-dark").on("click", function () {
        modal.show();
    });

    $(".btn-secondary").on('click', function () {
        modal.hide();
    });

});
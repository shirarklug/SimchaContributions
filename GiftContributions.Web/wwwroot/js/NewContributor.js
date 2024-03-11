$(() => {
    const modal = new bootstrap.Modal("#new-contributor");
    $(".btn-outline-dark").on("click", function () {
        modal.show();
    });

    $(".btn-secondary").on('click', function () {
        modal.hide();
    });

});

$(() => {
    const modal = new bootstrap.Modal("#add-deposit");
    $(".deposit").on('click', function () {
        const button = $(this);
        const contributorId = button.data('contributor-id');
        $("#contributor-id-hidden").val(contributorId);
        modal.show();
    });

    $(".btn-secondary").on('click', function () {
        modal.hide();
    });

});
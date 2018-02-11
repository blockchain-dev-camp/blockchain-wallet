
window.addEventListener('load', function () {

    alert('hi');

    $('input[type="submit"]').click(function() {
        $('.transaction-confirmation-controls').hide();
    })
});

window.addEventListener('load', function () {

    $('#btn-get-balance').click(getBalance);


    function getBalance() {
        // 9a9f082f37270ff54c5ca4204a0e4da6951fe917
        
        let data = {
            account: "9a9f082f37270ff54c5ca4204a0e4da6951fe917"
        }
        
        app.makeRequest(app.url.getBalance, data, 'POST')
            .then(res => {

                if (res.isSuccess) {
                    $('#account-address').text(data.account);
                    $('#user-balance').val(res.balance);
                }

            })
            .catch(err => {
                console.log(err);
            });
    }
});
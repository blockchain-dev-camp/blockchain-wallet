
window.addEventListener('load', function () {

    //$('#btn-get-balance').click(getBalance);
    //console.log(app.bool.shouldCheckBalance);

    if (app.bool.shouldCheckBalance) {
        getBalance();
    }


    function getBalance() {
        // 9a9f082f37270ff54c5ca4204a0e4da6951fe917

        app.startLogo();

        let data = {
            account: app.id.account
        }
        
        app.makeRequest(app.url.getBalance, data, 'POST')
            .then(res => {

                if (res.isSuccess) {
                    console.log('success');
                    //$('#account-address').val(data.account);
                    $('#user-balance').val(res.balance);
                } else {
                    console.log('error !');
                }

                app.stopLogo();

            })
            .catch(err => {
                app.stopLogo();
                console.log(err);
            });
    }
});
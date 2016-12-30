var messageForm = new Vue({
    el: '#newMessageContainer',
    data: {
        body: '',
        tags: [],
        maxlength: 150
    },
    computed: {
        remaining: function () {
            return (this.maxlength - this.body.length);
        }
    },
    methods: {
        SubmitNewMessage: function () {
            var newMessageData = {
                Body: this._data.body,
                Tags: this._data.tags,
                //authorId: //some author Id
            }
            axios({
                method: 'POST',
                url: 'http://localhost:62896/api/v1/messages/new',
                data: newMessageData,
                headers: {
                    'Authorization': ''
                },
                responseType: 'json'
            }).then(function (response) {
                console.log(response);
            });
        }
    }
});

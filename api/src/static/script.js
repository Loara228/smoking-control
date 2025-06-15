document.getElementById('form1').addEventListener('submit', function(event) {
    event.preventDefault();

    const formData = new FormData(this);

    fetch('/site/registration/post', {
        method: this.method,
        body: formData
    })
    .then(response => {
        if (response.ok) {
            return response.text();
        }
        throw new Error('something wrong');
    })
    .then(_data => {
        console.log('1');
    })
    .catch(_error => {
        console.log('0');
    });
});
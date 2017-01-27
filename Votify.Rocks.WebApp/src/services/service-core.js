
const get = (url, params) => {
    let headers = createHeaders();
    let getUrl = url+paramsFromObject(params);

    return fetch(getUrl, {
        method: 'get',
        headers: headers
    })
    .then(function (response) {
        if(!response.ok){
            throw response.status + ':' + response.statusText;
        }
        return response.json();
    })
    .catch(function (error) {
        throw error.message || error;
    });
};

const post = (url, params, data) => {
    let headers = createHeaders();
    let postUrl = url+paramsFromObject(params);

    return fetch(postUrl, {
        method: 'post',
        headers: headers,
        body: JSON.stringify(data)
    })
    .then(function (response) {
        if(!response.ok){
            throw response.status + ':' + response.statusText;
        }
        if(response.status === 204){
            return {};
        }
        return response.json();
    })
    .catch(function (error) {
        throw error.message || error;
    });
};

const put = (url, params, data) => {
    console.error('not implimented!');
};

const patch = (url, params, data) => {
    let headers = createHeaders();
    let patchUrl = url+paramsFromObject(params);

    return fetch(patchUrl, {
        method: 'PATCH',
        headers: headers,
        body: JSON.stringify(data)
    })
    .then(function (response) {
        if(!response.ok){
            throw response.status + ':' + response.statusText;
        }
        if(response.status === 204){
            return {};
        }
        return response.json();
    })
    .catch(function (error) {
        throw error.message || error;
    });
};

const del = (url, params, data) => {
    console.error('not implimented!');
};

const paramsFromObject = (params) => {
    if(!params){
        return '';
    }

    let paramArray = Object.keys(params).reduce((previous, key) => {
        let val = params[key];
        if (typeof val === 'string') {
            val = encodeURIComponent(val);
        }
        previous.push(key+'='+params[key]);
        return previous;
    }, []);

    return '?'+paramArray.join('&');
};

const createHeaders = () => {
    return new Headers({
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    });
};

export {get, put, post, patch, del};
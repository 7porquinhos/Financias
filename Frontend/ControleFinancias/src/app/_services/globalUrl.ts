
export class GlobalUrl {
    /*URL Dev */
    //_baseURL = 'http://localhost:44323/api/';

    /*URL Prod */
    _baseURL = 'http://192.168.0.121:9011/api/';
    
    constructor(BaseURL: string) 
    { 
        this._baseURL = this._baseURL + BaseURL;
    }
}

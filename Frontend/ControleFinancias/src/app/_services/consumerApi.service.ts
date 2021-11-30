import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GlobalUrl } from './globalUrl';

@Injectable({
  providedIn: 'root'
})
export class ConsumerApiService<T>{
  baseURL = new GlobalUrl('UserLogin');

  constructor(private http: HttpClient) {}

  getAll(): Observable<T[]> {

   return this.http.get<T[]>(this.baseURL._baseURL);
  }

   getById(id: number): Observable<T> {
    return this.http.get<T>(this.baseURL._baseURL + id);
   }

   post(objeto: T) {
    return this.http.post(this.baseURL._baseURL, objeto);
   }

   put(objeto: T, id: number) {
    return this.http.put(this.baseURL._baseURL + '?Id=' + id, objeto);
   }

   delete(id: number) {
    //return this.http.put(this.baseURL +'?EventoId='+ evento.id, evento);
    return this.http.delete(this.baseURL._baseURL + '?Id=' + id);
   }

}

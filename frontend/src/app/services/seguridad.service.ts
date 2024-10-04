import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class SeguridadService {
  url: string= environment.url;

  constructor(private http: HttpClient) { }

  validarAcceso(user: string, pass: string){
    const data: any = {
      "mail": user,
      "password": pass
    }
    return this.http.post(`${ this.url }/Login`, data);
    // return this.http.post(`${ this.url }/Login`, data, {responseType: 'text'});
  }

  sendEnvioCorreo(ruc: string, code: string){
    const data: any = {
      "code": code,
      "mail": ruc
    }
    return this.http.post(`${ this.url }/Password/PassLost`, data, {responseType: 'text'});
  }
}

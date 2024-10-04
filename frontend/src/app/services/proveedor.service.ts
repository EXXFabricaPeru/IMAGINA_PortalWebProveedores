import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class ProveedorService {
  url: string= environment.url;

  constructor(private http: HttpClient) { }

  getCliente(id: string){
    //id="P20100067839";
    return this.http.get(`${ this.url }/Socios/${ id }`)
  }

  getProveedor(id: string){
    //id="P20100067839";
    return this.http.get(`${ this.url }/Socios/Draft/${ id }`)
  }

  getListaProveedor(valor: string, estado: string){
    // console.log("url", `${ this.url }/Socios/Lista/${ valor }`);    
    return this.http.get(`${ this.url }/Socios/Lista?valor=${ valor }&estado=${ estado }`)
  }

  getListaProveedorFact(){   
    return this.http.get(`${ this.url }/Socios/Lista/Factoring`)
  }

  changePassword(data: any){
    return this.http.post(`${ this.url }/Password`, data, {responseType: 'text'});
  }

  crearProveedor(data: any){
    return this.http.post(`${ this.url }/Socios`, data, {responseType: 'text'});
  }

  aprobarProveedor(data: any){
    return this.http.post(`${ this.url }/Socios/Aprobar`, data, {responseType: 'text'});
  }

  downloadFile(code: string, ruc: string){
    return this.http.get(`${ this.url }/Socios/Archivo/${ code }, ${ ruc }`, {responseType: 'text'});
  }

  downloadFormato(name: string){
    return this.http.get(`${ this.url }/Socios/Formato/${ name }`, {responseType: 'text'});
  }
}

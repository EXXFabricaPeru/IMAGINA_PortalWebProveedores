import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class MaestroService {

  url: string = environment.url+"/Maestro";

  constructor(private http: HttpClient) { }

  getCondicionesPago(){
    return this.http.get(`${ this.url }/CondicionPago`);
  }

  getMoneda(){
    return this.http.get(`${ this.url }/Moneda`);
  }

  getBanco(){
    return this.http.get(`${ this.url }/Banco`);
  }

  getTipoCuenta(){
    return this.http.get(`${ this.url }/TipoCuenta`);
  }

  getDepartamento(){
    return this.http.get(`${ this.url }/Departamento/PE`);
  }

  getProvincia(id: string){
    return this.http.get(`${ this.url }/Provincia/${ id }`);
  }

  getDistrito(id: string){
    return this.http.get(`${ this.url }/Distrito/${ id }`);
  }

  getConfiguracion(){
    return this.http.get(`${ this.url }/Configuracion`);
  }

  getProveedorFactoring(){
    return this.http.get(`${ this.url }/ProvFact`);
  }

  getFormatos(){
    return this.http.get(`${ this.url }/Formatos`);
  }
}

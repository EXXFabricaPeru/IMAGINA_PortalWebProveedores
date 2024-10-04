import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { environment } from '../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class DocumentoService {

  url: string= environment.url;

  constructor(private http: HttpClient) { }

  getPedidos(ruc: string, fecIni: string, fecFin: string, estado: string){
    return this.http.get(`${ this.url }/Pedido/Lista/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`);
  }

  getPedidosDownload(ruc: string, fecIni: string, fecFin: string, estado: string){
    return this.http.get(`${ this.url }/Pedido/ListaDescargar/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`, {responseType: 'text'});
  }

  getPedidoId(id: string){
    return this.http.get(`${ this.url }/Pedido/${ id }`);
  }

  confirmarOC(data: any){
    return this.http.post(`${ this.url }/Pedido/Confirmar`, data, {responseType: 'text'});
  }

  getConformidad(ruc: string, fecIni: string, fecFin: string, estado: string){
    return this.http.get(`${ this.url }/Pedido/Conformidad/Lista/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`);
  }

  getConformidadDownload(ruc: string, fecIni: string, fecFin: string, estado: string){
    return this.http.get(`${ this.url }/Pedido/Conformidad/ListaDescargar/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`, {responseType: 'text'});
  }

  getConformidadDisponible(ruc: string, fecIni: string, fecFin: string, sucursal: string){
    return this.http.get(`${ this.url }/Pedido/Conformidad/Disponible/${ ruc }/${ fecIni }/${ fecFin }/${ sucursal }`);
  }

  getConformidadId(id: string){
    return this.http.get(`${ this.url }/Pedido/Conformidad/Id/${ id }`);
  }

  getConformidadAprId(id: string){
    return this.http.get(`${ this.url }/Pedido/ConformidadApr/Id/${ id }`);
  }

  aprobarConformidad(data: any){
    return this.http.post(`${ this.url }/Pedido/AprobarCS`, data, {responseType: 'text'});
  }

  getFacturas(ruc: string, fecIni: string, fecFin: string, estado: string){   
    return this.http.get(`${ this.url }/Factura/Lista/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`);
  }

  getFacturasDownload(ruc: string, fecIni: string, fecFin: string, estado: string){   
    return this.http.get(`${ this.url }/Factura/ListaDescargar/${ ruc }/${ fecIni }/${ fecFin }?estado=${ estado }`, {responseType: 'text'});
  }

  crearFactura(data: any){
    return this.http.post(`${ this.url }/Factura/Crear`, data, {responseType: 'text'});
  }

  actualizarFactura(data: any){
    return this.http.put(`${ this.url }/Factura/Update`, data, {responseType: 'text'});
  }

  crearFacturaAnticipo(data: any){
    return this.http.post(`${ this.url }/Factura/CrearAnticipo`, data, {responseType: 'text'});
  }

  getFacturaId(id: string){
    return this.http.get(`${ this.url }/Factura/${ id }`);
  }

  getAnticipoId(id: string){
    return this.http.get(`${ this.url }/Factura/Anticipo/${ id }`);
  }

  getBorradorId(id: string){
    return this.http.get(`${ this.url }/Factura/Borrador/${ id }`);
  }

  downloadFile(code: string){
    return this.http.get(`${ this.url }/Pedido/Archivo/${ code }`, {responseType: 'text'});
  }

  //Validacion de Facturas en SUNAT
  //Login a SUNAT
  obtenerTokenSunat(){
    const _url: string = "https://api-seguridad.sunat.gob.pe/v1/clientesextranet/0f08bc39-d50b-4363-95a4-aaaa865ac3ba/oauth2/token/"
    
    let body = new URLSearchParams();
    body.set("grant_type", "client_credentials");
    body.set("scope", "https://api.sunat.gob.pe/v1/contribuyente/contribuyentes");
    body.set("client_id", "0f08bc39-d50b-4363-95a4-aaaa865ac3ba");
    body.set("client_secret", "vdvlnWrPsUnd6HhVCjVhDg==");

    let options = {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    };

    return this.http.post(_url, body, options);
  }

  //Consulta del documento
  validarFactura(data: any, token: string){
    const _url: string = `https://api.sunat.gob.pe/v1/contribuyente/contribuyentes/${ data.numRuc }/validarcomprobante`

    let options = {
      headers: new HttpHeaders().set('Authorization', `Bearer ${ token }`)
    };

    return this.http.post(_url, data, { headers: { 'Authorization': `Bearer ${token}` } })
  }
}

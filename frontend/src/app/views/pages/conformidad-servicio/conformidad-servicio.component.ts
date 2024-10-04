import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { PagProveedorPipe } from 'src/app/pipes/pag-proveedor.pipe';

@Component({
  selector: 'app-conformidad-servicio',
  standalone: true,
  imports: [TextColorDirective,
    CardComponent,
    CardHeaderComponent,
    CardBodyComponent,
    CommonModule,
    AlertComponent,
    RowComponent,
    ColComponent,
    InputGroupComponent,
    CardGroupComponent,
    ContainerComponent,
    FormDirective,
    InputGroupTextDirective, 
    FormControlDirective, 
    ButtonDirective,
    IconDirective,
    TableModule, 
    UtilitiesModule,
    NgStyle,
    PagProveedorPipe],
  templateUrl: './conformidad-servicio.component.html',
  styleUrl: './conformidad-servicio.component.scss'
})
export class ConformidadServicioComponent implements OnInit {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagFiltro: Boolean = false;
  msgError: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  listaPaginas: number[] = [];
  pagina: number = 1;
  fecDesde: string = "";
  fecHasta: string = "";
  
  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;

    const token: any = sessionStorage.getItem("tb");

    var fecha = new Date();
    var date = new Date();
    var primerDia = new Date(date.getFullYear(), date.getMonth(), 1);
    var ultimoDia = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    
    // console.log("<br>El primer día es: "+primerDia.getDate().toString().padStart(2, "0"));
    // console.log("<br>El ultimo día es: "+ultimoDia.getDate());
    // const fecha = new Date().getFullYear().toString() + "-" + new Date().getMonth().toString().padStart(2 , "0") + "-01"
    console.log("fecha->", fecha.toISOString());
    
    this.fecDesde = fecha.toISOString().slice(0, 8) + primerDia.getDate().toString().padStart(2, "0");
    this.fecHasta = fecha.toISOString().slice(0, 8) + ultimoDia.getDate().toString().padStart(2, "0");

    if(token){
      this.listaHeader = [
        {
          label: "Nro Conformidad",
          key: "docNum",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Nro OC",
          key: "numeroCotizacion",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "N° Guia",
          key: "numAtCard",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Sucusal",
          key: "sucursal",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Fecha",
          key: "docDate",
          subKey: "",
          customClass: "fecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Moneda",
          key: "docCur",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Importe",
          key: "docTotal",
          subKey: "",
          customClass: "derecha",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Estado",
          key: "docStatus",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "",
          key: "nroPedido",
          subKey: "",
          customClass: "btnEdit",
          type: "buttonSearch",
          value: "",
          visible: true
        },
      ];
    }else{
      this.router.navigateByUrl(`/login`, { replaceUrl: true });
    }
  }

  async buscar(){
    this.listaPedidos = [];
    this.listaPaginas = [];
    this.flagFiltro = false;
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");
    // console.log("ruc", this._rucProveedor)    
    // const data: any =
     await this.documentoService.getConformidad(this._rucProveedor, fi, ff, cmbEstado.value).toPromise().then(pedidos => {
      // console.log("conformidad->", pedidos);
      this.listaPedidos = pedidos;
      
      if(this.listaPedidos.length == 0){
        this.flagFiltro = true;
        return;
      }
      const residuo: number = this.listaPedidos.length % 10;
      const cociente: string = (this.listaPedidos.length / 10).toString().split('.')[0];
      const x: number = residuo == 0 ? 0 : 1;
      for(let i = 1; i <= Number(cociente) + x; i++){
        this.listaPaginas.push(i);
      }

      this.pagina = 1;
     });    
  }

  ngOnInit(): void {
    
  }

  searchRow(row: any){
    const docEntry: number = row.docEntry
    const docNum = row.docNum.toString().split("-");
    let tipo: string = "";

    if(docNum.length == 1)
      tipo = "C";
    else
      tipo = "B"

    this.router.navigateByUrl(`/conformidad-servicio-mostrar/${ docEntry }/${ tipo }`, { replaceUrl: true });
  }

  selectPagina(num: number){
    this.pagina = num;
  }

  async descargar(){
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");

    this.documentoService.getConformidadDownload(this._rucProveedor, fi,ff,cmbEstado.value).toPromise().then( data => {
      console.log("file", data);
      const archivo = data || "";
      var byteCharacters = atob(archivo.toString());
      var byteNumbers = new Array(byteCharacters.length);

      for (var i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers); 
 
      let filename = "pedido_" + fi + ff + ".xlsx";  
      let binaryData = [];
      binaryData.push(byteArray);
      
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(
      new Blob(binaryData, { type: 'blob' }));
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
    });
  }

}

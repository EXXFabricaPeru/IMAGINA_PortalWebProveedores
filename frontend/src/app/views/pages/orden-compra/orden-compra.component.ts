import { Component, OnInit } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';
import { PagProveedorPipe } from 'src/app/pipes/pag-proveedor.pipe';

@Component({
  selector: 'app-orden-compra',
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
    PagProveedorPipe
  ],
  templateUrl: './orden-compra.component.html',
  styleUrl: './orden-compra.component.scss'
})
export class OrdenCompraComponent implements OnInit {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  flagFiltro: Boolean = false;
  msgError: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  fecDesde: string = "";
  fecHasta: string = "";
  listaPaginas: number[] = []

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    // const dataTemp: any = localStorage.getItem("prov");
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

    console.log(this.fecDesde, this.fecHasta);
    

    if(token){
      this.listaHeader = [
        {
          label: "NroPed",
          key: "docNum",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Nro Ref.",
          key: "numAtCard",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Sucursal",
          key: "sucursal",
          subKey: "",
          customClass: "",
          type: "",
          value: "",
          visible: true
        },
        {
          label: "Cond. Pag.",
          key: "condicionPago",
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
          label: "Fec. Necesaria",
          key: "docDueDate",
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

  ngOnInit(): void {
    
  }

  async buscar(){
    this.listaPaginas = [];
    this.listaPedidos = [];
    this.flagFiltro = false;
    const txtDesde = document.getElementById("txtDesde") as HTMLInputElement;
    const txtHasta = document.getElementById("txtHasta") as HTMLInputElement;
    const cmbEstado =  document.getElementById("cmbEstado") as HTMLSelectElement;

    const fi: string = txtDesde.value.replace("-","").replace("-","");
    const ff: string = txtHasta.value.replace("-","").replace("-","");
    // console.log(fi,ff);
      
    // console.log("ruc", this._rucProveedor)    
    // const data: any =
     await this.documentoService.getPedidos(this._rucProveedor, fi, ff, cmbEstado.value).toPromise().then(pedidos => {
      // console.log("pedidos->", pedidos);
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
    // console.log(data);
    // const dataJson: any = JSON.parse(data);
    // const pedidos: any = dataJson.__zone_symbol__value;
    // console.log("pedidos->", pedidos);
  }

  async searchRow(row: any){
    const docEntry: number = row.docEntry
    this.router.navigateByUrl(`/orden-compra-mostrar/${ docEntry }`, { replaceUrl: true });
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

    this.documentoService.getPedidosDownload(this._rucProveedor, fi,ff,cmbEstado.value).toPromise().then( data => {
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

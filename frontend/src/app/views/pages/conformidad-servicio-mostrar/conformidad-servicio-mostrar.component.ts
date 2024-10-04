import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';

@Component({
  selector: 'app-conformidad-servicio-mostrar',
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
    NgStyle],
  templateUrl: './conformidad-servicio-mostrar.component.html',
  styleUrl: './conformidad-servicio-mostrar.component.scss'
})
export class ConformidadServicioMostrarComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  msgError: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  pedido: any = {docNum: ""};
  _docentry: string = "";

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;

    this.listaHeader = [
      {
        label: "Código",
        key: "itemCode",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Descripción",
        key: "dscription",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Cantidad",
        key: "quantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      // {
      //   label: "Cant. Pend.",
      //   key: "pendQuantity",
      //   subKey: "",
      //   customClass: "derecha",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      {
        label: "U.M",
        key: "unitMsr",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      // {
      //   label: "Fec. Entrega",
      //   key: "shipDate",
      //   subKey: "",
      //   customClass: "fecha",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      // {
      //   label: "Impuesto",
      //   key: "taxCode",
      //   subKey: "",
      //   customClass: "",
      //   type: "",
      //   value: "",
      //   visible: true
      // },
      {
        label: "Prec. Unit",
        key: "price",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Precio Total",
        key: "lineTotal",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
    ];
    
    let docEntry: string | null;

    docEntry = this._route.snapshot.paramMap.get("id") || "";
    this._docentry = docEntry;
    this.obtenerPedido(docEntry);
  }

  async obtenerPedido(id: string){
    let tipo: string | null;

    tipo = this._route.snapshot.paramMap.get("tipo") || "";

    if(tipo == "C"){
      await this.documentoService.getConformidadId(id).toPromise().then(data => {
        // console.log("CS", data);
        this.pedido = data;
        this.listaPedidos = this.pedido.detallePedido;
      });
    }else{
      await this.documentoService.getConformidadAprId(id).toPromise().then(data => {
        // console.log("CS", data);
        this.pedido = data;
        this.listaPedidos = this.pedido.detallePedido;
      });
    }

    
  }

  crearConformidadServicio(){
    this.router.navigateByUrl(`/factura-crear/${ this._docentry }`, { replaceUrl: false });
  }

  downloadFile(code: string) {
    let nombre: string = "";
    let archivo: string = "";
    switch (code)
    {
      case "1":
        nombre = this.pedido.nomArchivo;
        archivo = this.pedido.archivo.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "2":
        nombre = this.pedido.nomArchivo2;
        archivo = this.pedido.archivo2.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
      case "3":
        nombre = this.pedido.nomArchivo3;
        archivo = this.pedido.archivo3.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");
        break;
    }

    this.documentoService.downloadFile(archivo).toPromise().then(data => {
      // console.log("archivo", data);
      const archivo: string = data || "";
      // this.pdfSrc = archivo;
      // console.log("archivo", this.pdfSrc);
      var byteCharacters = atob(archivo);
      var byteNumbers = new Array(byteCharacters.length);

      for (var i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      var byteArray = new Uint8Array(byteNumbers); 
 
      let filename = nombre;  
      let binaryData = [];
      binaryData.push(byteArray);
      
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(
      new Blob(binaryData, { type: 'blob' }));
      downloadLink.setAttribute('download', filename);
      document.body.appendChild(downloadLink);
      downloadLink.click();
      // this.confirmDialog = true;
    })
  }
}

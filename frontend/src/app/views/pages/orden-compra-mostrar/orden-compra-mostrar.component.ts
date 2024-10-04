import { Component } from '@angular/core';
import { TextColorDirective, CardComponent, CardHeaderComponent, CardBodyComponent, AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, 
  FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective,
  TableModule, UtilitiesModule } from '@coreui/angular';
import { NgStyle, CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentoService } from 'src/app/services/documento.service';

@Component({
  selector: 'app-orden-compra-mostrar',
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
  templateUrl: './orden-compra-mostrar.component.html',
  styleUrl: './orden-compra-mostrar.component.scss'
})
export class OrdenCompraMostrarComponent {
  errorValidacion: Boolean = false;
  flagExito: Boolean = false;
  msgError: string = "";
  listaHeader: any[] = [];
  _rucProveedor: string = "";
  listaPedidos: any = [];
  pagina: number = 1;
  pedido: any = { docNum: "" };
  _docentry: string = "";
  formatPrec: string = "";
  formatCant: string = "";
  decimalPrec: number = 0;
  decimalCant: number = 0;

  constructor(private _route: ActivatedRoute, private router: Router, private documentoService: DocumentoService){
    // const dataTemp: any = localStorage.getItem("prov");
    const dataTemp: any = sessionStorage.getItem("prov");
    const dataProv: any = JSON.parse(dataTemp);
    // console.log("prov", dataProv)
    this._rucProveedor = dataProv.licTradNum;

    const dataTempC: any = sessionStorage.getItem("config");
    const dataConfig: any = JSON.parse(dataTempC);

    for(let i=0; i<dataConfig.length; i++){
      if(dataConfig[i].descripcion == "PrecioDec")
        this.decimalPrec = dataConfig[i].valor_01

      if(dataConfig[i].descripcion == "CantidadDec")
        this.decimalCant = dataConfig[i].valor_01
    }

    this.formatPrec = `1-${this.decimalPrec}-${this.decimalPrec}`
    this.formatCant = `1-${this.decimalCant}-${this.decimalCant}`

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
        key: "cantidad",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Cant. Pend.",
        key: "pendQuantity",
        subKey: "",
        customClass: "derecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "U.M",
        key: "unitMsr",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Fec. Entrega",
        key: "shipDate",
        subKey: "",
        customClass: "fecha",
        type: "",
        value: "",
        visible: true
      },
      {
        label: "Impuesto",
        key: "taxCode",
        subKey: "",
        customClass: "",
        type: "",
        value: "",
        visible: true
      },
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
    await this.documentoService.getPedidoId(id).toPromise().then(data => {
      console.log("pedido", data);
      this.pedido = data;
      this.listaPedidos = this.pedido.detallePedido;
      this.pedido.subTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal) - Number(this.pedido.vatSum), );
      this.pedido.docTotal = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.docTotal), );
      this.pedido.vatSum = new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.pedido.vatSum), );
      for(let i=0; i<this.listaPedidos.length; i++){
        this.listaPedidos[i].price = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: this.decimalPrec }).format(Number(this.listaPedidos[i].price), )).toString();
        this.listaPedidos[i].lineTotal = (new Intl.NumberFormat("es-PE", { minimumFractionDigits: 2 }).format(Number(this.listaPedidos[i].lineTotal), )).toString();
      }
      // console.log(this.decimalPrec, new Intl.NumberFormat("es-PE", { minimumFractionDigits: this.decimalPrec }).format(Number(this.pedido.docTotal), ), );
    });

    console.log(this.pedido);
    
  }

  crearConformidadServicio(){
    this.router.navigateByUrl(`/conformidad-servicio-crear/${ this._docentry }`, { replaceUrl: false });
  }

  crearAnticipo(){
    this.router.navigateByUrl(`/anticipo-crear/${ this._docentry }`, { replaceUrl: false });
  }

  download() {
    let nombre: string = this.pedido.docNum + ".pdf";
    let archivo: string = this.pedido.archivo.toString().replace("\\\\", "||").replace("\\", "|").replace("\\", "|");

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

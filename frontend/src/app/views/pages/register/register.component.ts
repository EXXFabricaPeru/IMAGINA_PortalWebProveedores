import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, 
        FormControlDirective, ButtonDirective, CardHeaderComponent, AlertComponent, FormCheckComponent, ModalModule} from '@coreui/angular';
import { MaestroService } from 'src/app/services/maestro.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ProveedorService } from 'src/app/services/proveedor.service';
import { Router } from '@angular/router';


@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    standalone: true,
    imports: [ContainerComponent, RowComponent, ColComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, CardHeaderComponent,
      CommonModule, AlertComponent, FormCheckComponent, ModalModule
    ]
})
export class RegisterComponent {
  tab1: boolean = true;
  tab2: boolean = false;
  tab3: boolean = false;
  tab4: boolean = false;
  confirmDialog: boolean = false;
  flagExito: boolean = false;
  flagLoad: boolean = false;

  listaContacto: any[] = [];
  listaCondicionPago: any;
  listaTipoCuenta: any;
  listaMoneda: any;
  listaDireccion: any[] = [];
  listaCuenta: any = [];
  listaFormato: any = [];
  listaBanco: any;
  listaDepartamento: any;
  listaProvincia: any;
  listaDistrito: any = [];
  listaFormatos: any;
  
  provinciaSel: string = "";
  distritoSel: string = "";
  distSel: string = "";
  textButtonContacto: string = "Agregar";
  textButtonDireccion: string = "Agregar";
  textButtonCuenta: string = "Agregar";
  msgError: string = "";

  archivo1: any;
  nomArchivo1: string = "";
  archivo2: any;
  nomArchivo2: string = "";
  archivo3: any;
  nomArchivo3: string = "";
  archivo4: any;
  nomArchivo4: string = "";
  archivo5: any;
  nomArchivo5: string = "";
  archivo6: any;
  nomArchivo6: string = "";
  archivo7: any;
  nomArchivo7: string = "";
  archivo8: any;
  nomArchivo8: string = "";
  archivo9: any;
  nomArchivo9: string = "";
  archivo10: any;
  nomArchivo10: string = "";
  // archivo11: any;
  // nomArchivo11: string = "";
  // archivo12: any;
  // nomArchivo12: string = "";
  // archivo13: any;
  // nomArchivo13: string = "";
  // archivo14: any;
  // nomArchivo14: string = "";
  archivoaux1: any;
  nomArchivoaux1: string = "";
  flagArchivoaux1: boolean = false;
  archivoaux2: any;
  nomArchivoaux2: string = "";
  flagArchivoaux2: boolean = false;
  archivoaux3: any;
  nomArchivoaux3: string = "";
  flagArchivoaux3: boolean = false;
  archivoaux4: any;
  nomArchivoaux4: string = "";
  flagArchivoaux4: boolean = false;
  archivoaux5: any;
  nomArchivoaux5: string = "";
  flagArchivoaux5: boolean = false;
  
  constructor(private maestroService: MaestroService, private sanitizer: DomSanitizer, private proveedorService: ProveedorService, private router: Router) { 
    maestroService.getConfiguracion().toPromise().then(data => {
      console.log("configuracion", data);
       const dataConfig: any = data;
      for(let i=0; i<dataConfig.length; i++){
        if(i==0){
          this.nomArchivoaux1 = dataConfig[i].descripcion;
          this.flagArchivoaux1 = dataConfig[i].flag01;
        }
        if(i==1){
          this.nomArchivoaux2 = dataConfig[i].descripcion;
          this.flagArchivoaux2 = dataConfig[i].flag01;
        }
        if(i==2){
          this.nomArchivoaux3 = dataConfig[i].descripcion;
          this.flagArchivoaux3 = dataConfig[i].flag01;
        }
        if(i==3){
          this.nomArchivoaux4 = dataConfig[i].descripcion;
          this.flagArchivoaux4 = dataConfig[i].flag01;
        }
        if(i==4){
          this.nomArchivoaux5 = dataConfig[i].descripcion;
          this.flagArchivoaux5 = dataConfig[i].flag01;
        }
      }
    })

    maestroService.getCondicionesPago().toPromise().then(data => {
      this.listaCondicionPago = data;
      // console.log(this.listaCondicionPago);
    });
    
    maestroService.getMoneda().toPromise().then(data => {
      this.listaMoneda = data;
      // console.log(this.listaMoneda);
    });    
    
    maestroService.getDepartamento().toPromise().then(data => {
      this.listaDepartamento = data;
      // console.log(this.listaDepartamento);
    });
        
    maestroService.getBanco().toPromise().then(data => {
      this.listaBanco = data;
      // console.log(this.listaDepartamento);
    });    
        
    maestroService.getTipoCuenta().toPromise().then(data => {
      this.listaTipoCuenta = data;
      // console.log(this.listaDepartamento);
    }); 
        
    // console.log("--------");
    
    maestroService.getFormatos().toPromise().then(data => {
      this.listaFormatos = data;
      // console.log(this.listaFormatos);
    }); 

  }

  agregarContacto(){
    const _txtIdContacto = document.getElementById("txtCodContacto") as HTMLInputElement;
    const _txtPrimerNombre = document.getElementById("txtPrimerNombre") as HTMLInputElement;
    const _txtSegundNombre = document.getElementById("txtSegundoNombre") as HTMLInputElement;
    const _txtApellido = document.getElementById("txtApellidoCont") as HTMLInputElement;
    const _txtTelefono = document.getElementById("txtTelfContacto") as HTMLInputElement;
    const _txtCelular = document.getElementById("txtCelContacto") as HTMLInputElement;
    const _txtCargo = document.getElementById("txtCargoContac") as HTMLInputElement;
    const _txtCorreo = document.getElementById("txtEmailContacto") as HTMLInputElement;

    let nombreContacto: string = _txtIdContacto.value;    
    let primerNombreContacto: string = _txtPrimerNombre.value;
    let segundoNombreContacto: string = _txtSegundNombre.value;
    let apellidoContacto: string = _txtApellido.value;
    let telefonoContacto: string = _txtTelefono.value;
    let celularContacto: string = _txtCelular.value;
    let cargoContacto: string = _txtCargo.value;
    let correoContacto: string = _txtCorreo.value;

    if(nombreContacto == ""){
      this.msgError = "Debe colocar un ID de Contacto";
      this.confirmDialog = true;
      return;
    }

    if(primerNombreContacto == ""){
      this.msgError = "Debe colocar el primer nombre del contacto";
      this.confirmDialog = true;
      return;
    }

    if(apellidoContacto == ""){
      this.msgError = "Debe colocar el apellido del Contacto";
      this.confirmDialog = true;
      return;
    }

    if(celularContacto == ""){
      this.msgError = "Debe colocar el celular del Contacto";
      this.confirmDialog = true;
      return;
    }
    
    if(correoContacto == ""){
      this.msgError = "Debe colocar el correo del Contacto";
      this.confirmDialog = true;
      return;
    }

    if(telefonoContacto == ""){
      this.msgError = "Debe colocar el teléfono del Contacto";
      this.confirmDialog = true;
      return;
    }

    if(cargoContacto == ""){
      this.msgError = "Debe colocar el cargo del Contacto";
      this.confirmDialog = true;
      return;
    }
    
    const _contacto: any = this.listaContacto.find((t: { nombre: any; })=>t.nombre == nombreContacto);
    if(this.textButtonContacto == "Agregar" && (_contacto != null || _contacto != undefined)){
      this.msgError = "El id de contacto ya existe";
      this.confirmDialog = true;
      return;
    }

    if(_contacto != null || _contacto != undefined){
      for(let i = 0; i < this.listaContacto.length; i++){
        if(nombreContacto == this.listaContacto[i].nombre){
          this.listaContacto[i].telefono = telefonoContacto;
          this.listaContacto[i].cargo = cargoContacto;
          this.listaContacto[i].email = correoContacto;
          this.listaContacto[i].flagEditar = true;
          this.listaContacto[i].primerNombre = primerNombreContacto;
          this.listaContacto[i].segundoNombre = segundoNombreContacto;
          this.listaContacto[i].apellido = apellidoContacto;
          this.listaContacto[i].celular = celularContacto;
          this.msgError = "Se actualizó con éxito el contacto";
        }
      }
    }else{
      const contacto: any = {
        nombre: nombreContacto,
        telefono: telefonoContacto,
        cargo: cargoContacto,
        email: correoContacto,
        flagEditar: true,
        flagEliminar: false,
        primerNombre: primerNombreContacto,
        segundoNombre: segundoNombreContacto,
        apellido: apellidoContacto,
        celular: celularContacto
      }
      console.log(contacto);      
      this.listaContacto.push(contacto);
      this.msgError = "Se agrego con éxito el contacto";
    }

    this.textButtonContacto = "Agregar";

    _txtCargo.value = "";
    _txtCorreo.value = "";
    _txtTelefono.value = "";
    _txtIdContacto.value = "";
    _txtPrimerNombre.value = "";
    _txtSegundNombre.value = "";
    _txtApellido.value = "";
    _txtCelular.value = "";
    _txtIdContacto.disabled = false;
  }

  editarContacto(contacto:any){
    const _txtIdContacto = document.getElementById("txtCodContacto") as HTMLInputElement;
    const _txtPrimerNombre = document.getElementById("txtPrimerNombre") as HTMLInputElement;
    const _txtSegundNombre = document.getElementById("txtSegundoNombre") as HTMLInputElement;
    const _txtApellido = document.getElementById("txtApellidoCont") as HTMLInputElement;
    const _txtTelefono = document.getElementById("txtTelfContacto") as HTMLInputElement;
    const _txtCelular = document.getElementById("txtCelContacto") as HTMLInputElement;
    const _txtCargo = document.getElementById("txtCargoContac") as HTMLInputElement;
    const _txtCorreo = document.getElementById("txtEmailContacto") as HTMLInputElement;

    this.textButtonContacto = "Actualizar";
    // console.log("contacto", contacto);

    _txtIdContacto.value = contacto.nombre;
    _txtTelefono.value = contacto.telefono;
    _txtCargo.value = contacto.cargo;
    _txtCorreo.value = contacto.email;
    _txtPrimerNombre.value = contacto.primerNombre;
    _txtSegundNombre.value = contacto.segundoNombre;
    _txtApellido.value = contacto.apellido;
    _txtCelular.value = contacto.celular;
    _txtIdContacto.disabled = true;
  }

  eliminarContacto(contactoEliminar: any){
    for(let i = 0; i< this.listaContacto.length; i++){
      if(this.listaContacto[i].nombre == contactoEliminar.nombre){
        // this.listaContacto[i].flagEliminar = true;
        this.listaContacto.splice(i, 1);
        break;
      }
    }
  }

  agregarDireccion(){
    const _cmbDepartamento = document.getElementById("cmbdepartamento") as HTMLSelectElement;
    const _cmbProvincia = document.getElementById("cmbprovincia") as HTMLSelectElement;
    const _cmbDistrito = document.getElementById("cmbdistrito") as HTMLSelectElement;
    const _cmbTipoDir = document.getElementById("cmbtipodir") as HTMLSelectElement;
    const _txtDireccion = document.getElementById("txtdireccion") as HTMLInputElement;
    const _txtIdDir = document.getElementById("input-id-dir") as HTMLInputElement;

    const departamentoSucursal: string = _cmbDepartamento.value;
    const provinciaSucursal: string = _cmbProvincia.value;
    const distritoSucursal: string = _cmbDistrito.value;
    const tipoDireccion: string = _cmbTipoDir.value; 
    const nombreDireccion: string = _txtIdDir.value;
    const direccionSucursal: string = _txtDireccion.value;
    
    if(departamentoSucursal == ""){
      this.msgError = "Debe seleccionar un departamento";
      this.confirmDialog = true;
      return;
    }

    if(provinciaSucursal == ""){
      this.msgError = "Debe seleccionar una provincia";
      this.confirmDialog = true;
      return;
    }
    
    if(distritoSucursal == ""){
      this.msgError = "Debe seleccionar un distrito";
      this.confirmDialog = true;
      return;
    }

    if(tipoDireccion == ""){
      this.msgError = "Debe seleccionar el tipo de dirección";
      this.confirmDialog = true;
      return;
    }

    if(direccionSucursal == ""){
      this.msgError = "Debe colocar la dirección";
      this.confirmDialog = true;
      return;
    }

    const _distrito: any = this.listaDistrito.find((t: { codigo: string; }) => t.codigo == distritoSucursal)
    this.distSel = _distrito.descripcion;

    const _direccion: any = this.listaDireccion.find(t=> t.address == nombreDireccion)
    if(_direccion != null && _direccion != undefined){
      for(let i = 0; i < this.listaDireccion.length; i++){
        if(nombreDireccion == this.listaDireccion[i].address){
          this.listaDireccion[i].departamento = departamentoSucursal;
          this.listaDireccion[i].provincia = provinciaSucursal;
          this.listaDireccion[i].distrito = distritoSucursal;
          this.listaDireccion[i].direccionDesc = direccionSucursal;
          this.listaDireccion[i].flagEditar = true;
          this.listaDireccion[i].nomDistrito = this.distSel;
          this.msgError = "Se actualizó con éxito la dirección";
        }
      }
    }else{
      const direccion: any = {
        address: nombreDireccion,
        departamento: departamentoSucursal,
        provincia: provinciaSucursal,
        distrito: distritoSucursal,
        adressType: tipoDireccion,
        direccionDesc: direccionSucursal,
        flagEditar: true,
        flagEliminar: false,
        nomDistrito: this.distSel
      }
  
      this.listaDireccion.push(direccion);
      this.msgError = "Se agrego con éxito la dirección";
    }

    _txtIdDir.value = "";
    _cmbDepartamento.value = "";
    _cmbProvincia.value = "";
    _cmbDistrito.value = "";
    _cmbTipoDir.value = "";
    _txtDireccion.value = "";

    this.listaProvincia = [];
    this.listaDistrito = [];
    this.textButtonDireccion = "Agregar";
    this.distSel = "";
    this.distritoSel = "";
    this.provinciaSel = "";
  }

  async editarDireccion(dir: any){
    const _cmbDepartamento = document.getElementById("cmbdepartamento") as HTMLSelectElement;
    const _cmbProvincia = document.getElementById("cmbprovincia") as HTMLSelectElement;
    const _cmbDistrito = document.getElementById("cmbdistrito") as HTMLSelectElement;
    const _cmbTipoDir = document.getElementById("cmbtipodir") as HTMLSelectElement;
    const _txtDireccion = document.getElementById("txtdireccion") as HTMLInputElement;
    const _txtIdDir = document.getElementById("input-id-dir") as HTMLInputElement;

    console.log("direccion", dir);
    this.textButtonDireccion = "Actualizar";
    _txtIdDir.value = dir.address;
    _txtDireccion.value = dir.direccionDesc;
    _cmbTipoDir.value = dir.adressType
    _cmbDepartamento.value = dir.departamento;
    this.provinciaSel = dir.provincia;
    this.distritoSel = dir.distrito;

    await this.maestroService.getProvincia(dir.departamento).toPromise().then(data => {
      this.listaProvincia = data;
    });
    _cmbProvincia.value = dir.provincia;

    await this.maestroService.getDistrito(dir.provincia).toPromise().then(data => {
      this.listaDistrito = data;
    });
    _cmbDistrito.value = dir.distrito;
  }

  eliminarDireccion(direccionElimar: any){
    for(let i = 0; i < this.listaDireccion.length; i++){      
      if(this.listaDireccion[i].address == direccionElimar.address){
        // this.listaDireccion[i].flagEliminar = true;
        this.listaDireccion.splice(i, 1);
        break;
      }
    }
    
    this.listaProvincia = [];
    this.listaDistrito = [];
  }

  obtenerDistrito(){
    const cmbprovincia = document.getElementById("cmbprovincia") as HTMLSelectElement
    this.maestroService.getDistrito(cmbprovincia.value).toPromise().then(data => {
      this.listaDistrito = data;
      // console.log(data);      
    })
  }

  obtenerProvincia(){
    const cmbdepartamento = document.getElementById("cmbdepartamento") as HTMLSelectElement
    this.maestroService.getProvincia(cmbdepartamento.value).toPromise().then(data => {
      this.listaProvincia = data;
      // console.log(data);
    })
  }

  obtenerCodigoDir(){
    const cmbTpoDir = document.getElementById("cmbtipodir") as HTMLSelectElement;
    console.log("Direcciones", this.listaDireccion);
    
    const list: any = this.listaDireccion.filter((t: { adressType: any; }) => t.adressType == cmbTpoDir.value);
    let nombreDireccion: string;
    console.log(list.length, list)
    if(cmbTpoDir.value == "B"){
      if(list.length == 0){
        nombreDireccion = "FISCAL"
      }else{
        let encontrado: Boolean = false;
        let nombreAux: string;
        nombreDireccion = "";
        for(let i = 0; i < list.length; i++){   
          nombreAux = "FISCAL" + (i+1).toString();            
          console.log(i, list.filter((t: { address: string; })=> t.address == nombreAux));
          if(encontrado == false){
            if(list.filter((t: { address: string; })=> t.address == nombreAux).length == 0){
              encontrado = true;
              nombreDireccion = nombreAux;
            }
          }
        }
        if(nombreDireccion == "")
          nombreDireccion = "FISCAL" + (list.length + 1).toString();
      }
    }else if(cmbTpoDir.value == "S"){
      if(list.length == 0){
        nombreDireccion = "ALMACEN"
      }else{
        let encontrado: Boolean = false;
        let nombreAux: string;
        nombreDireccion = "";
        for(let i = 0; i < list.length; i++){   
          nombreAux = "ALMACEN" + (i+1).toString();            
          console.log(i, list.filter((t: { address: string; })=> t.address == nombreAux));
          if(encontrado == false){
            if(list.filter((t: { address: string; })=> t.address == nombreAux).length == 0){
              encontrado = true;
              nombreDireccion = nombreAux;
            }
          }
        }
        if(nombreDireccion == "")
          nombreDireccion = "ALMACEN" + (list.length + 1).toString();
      }
    }else{
      nombreDireccion = "";
    }
    console.log(nombreDireccion);
    
    const _txtIdDir = document.getElementById("input-id-dir") as HTMLInputElement;
    _txtIdDir.value = nombreDireccion;
  }

  agregarCuenta(){
    const txtnomcuenta = document.getElementById("txtnomcuenta") as HTMLInputElement;
    const cmbbanco = document.getElementById("cmbbanco") as HTMLSelectElement;
    const cmbmoneda = document.getElementById("cmbmoneda") as HTMLSelectElement;
    const cmbtipocuenta = document.getElementById("cmbtipocuenta") as HTMLSelectElement;
    const txtnrocuenta = document.getElementById("txtnrocuenta") as HTMLInputElement;
    const txtinterbancaria = document.getElementById("txtinterbancaria") as HTMLInputElement;
    const cbIsDetraccion = document.getElementById("cbIsDetraccion") as HTMLInputElement;
    
    const nomCuenta: string = txtnomcuenta.value;
    const codBanco: string = cmbbanco.value;
    const moneda: string = cmbmoneda.value;
    const tipocuenta: string = cmbtipocuenta.value;
    const nroCuenta: string = txtnrocuenta.value;
    const interbancaria: string = txtinterbancaria.value;

    if(nomCuenta == ""){
      this.msgError = "Debe colocar un nombre de cuenta";
      this.confirmDialog = true;
      return;
    }

    if(codBanco == ""){
      this.msgError = "Debe seleccionar un banco";
      this.confirmDialog = true;
      return;
    }

    if(moneda == ""){
      this.msgError = "Debe seleccionar una moneda";
      this.confirmDialog = true;
      return;
    }

    if(tipocuenta == ""){
      this.msgError = "Debe seleccionar un tipo de cuenta";
      this.confirmDialog = true;
      return;
    }

    if(nroCuenta == ""){
      this.msgError = "Debe colocar un numero de cuenta";
      this.confirmDialog = true;
      return;
    }

    if(interbancaria == "" && cbIsDetraccion.checked == false){
      this.msgError = "Debe colocar un nunero de cuenta interbancaria";
      this.confirmDialog = true;
      return;
    }

    let xTipoCuenta: string = "";
    for(let x=0; x<this.listaTipoCuenta.length; x++){
      if(this.listaTipoCuenta[x].codigo == cmbtipocuenta.value){
        xTipoCuenta = this.listaTipoCuenta[x].descripcion;
        break;
      }
    }

    if(cbIsDetraccion.checked){
      if(txtnrocuenta.value.length != 11){
        this.msgError = "La cantidad de digitos para la cuenta de detraccion debe ser de 11";
        this.confirmDialog = true;
        return;
      }

      if(moneda != "SOL"){
        this.msgError = "La moneda debe ser SOL para detracción";
        this.confirmDialog = true;
        return;
      }
    }

    const _cuenta: any = this.listaCuenta.find((t: { acctName: string; })=> t.acctName == nomCuenta)

    if(this.textButtonCuenta == "Agregar" && (_cuenta != null && _cuenta != undefined)){
      this.msgError = "El nombre de la cuenta ya existe";
        this.confirmDialog = true;
        return;
    }

    if(_cuenta != null && _cuenta != undefined){
      for(let i = 0; i < this.listaCuenta.length; i++){
        if(nomCuenta == this.listaCuenta[i].acctName){
          this.listaCuenta[i].bankCode = codBanco,
          //this.listaCuenta[i].acctName = nomCuenta,
          this.listaCuenta[i].account = nroCuenta,
          this.listaCuenta[i].moneda= moneda,
          this.listaCuenta[i].tipo = tipocuenta,
          this.listaCuenta[i].u_EXM_INTERBANCARIA = interbancaria,
          this.listaCuenta[i].u_EXC_ACTIVO = "Y",
          this.listaCuenta[i].u_CurrSAP = moneda,
          this.listaCuenta[i].esDetraccion= cbIsDetraccion.checked
        }
      }
    }else{
      const esDetra: string = cbIsDetraccion.checked ? "Y" : "N";
      const cuenta: any = {
        bankCode: codBanco,
        acctName: nomCuenta,
        account: nroCuenta,
        moneda: moneda,
        tipo: tipocuenta,
        u_EXM_INTERBANCARIA: interbancaria,
        u_EXC_ACTIVO: "Y",
        u_CurrSAP: moneda,
        esDetraccion: cbIsDetraccion.checked
      };

      this.listaCuenta.push(cuenta)
    }

    this.textButtonCuenta = "Agregar"
    txtnomcuenta.value = "";
    cmbbanco.value = "";
    cmbmoneda.value = "";
    cmbtipocuenta.value = "";
    txtnrocuenta.value = "";
    txtinterbancaria.value = "";
    txtnomcuenta.disabled = false;
    cbIsDetraccion.checked = false;
  }

  editarCuenta(cuenta: any){
    const txtnomcuenta = document.getElementById("txtnomcuenta") as HTMLInputElement;
    const cmbbanco = document.getElementById("cmbbanco") as HTMLSelectElement;
    const cmbmoneda = document.getElementById("cmbmoneda") as HTMLSelectElement;
    const cmbtipocuenta = document.getElementById("cmbtipocuenta") as HTMLSelectElement;
    const txtnrocuenta = document.getElementById("txtnrocuenta") as HTMLInputElement;
    const txtinterbancaria = document.getElementById("txtinterbancaria") as HTMLInputElement;
    const cbIsDetraccion = document.getElementById("cbIsDetraccion") as HTMLInputElement;
    
    this.textButtonCuenta = "Actualizar";
    // console.log("cuenta", cuenta);

    txtnomcuenta.value = cuenta.acctName;
    cmbbanco.value = cuenta.bankCode;
    cmbmoneda.value = cuenta.moneda;
    cmbtipocuenta.value = cuenta.tipo;
    txtnrocuenta.value = cuenta.account;
    txtinterbancaria.value = cuenta.u_EXM_INTERBANCARIA;
    cbIsDetraccion.checked = cuenta.esDetraccion;

    txtnomcuenta.disabled = true;
  }

  eliminarCuenta(cuentaEliminar: any){
    for(let i = 0; i < this.listaCuenta.length; i++){      
      if(this.listaCuenta[i].acctName == cuentaEliminar.acctName){
        // this.listaCuenta[i].flagEliminar = true;
        this.listaCuenta.splice(i, 1);
        break;
      }
    }
  }

  onBlurDocumento(){
    console.log("blur");
    // debugger;
    const _cmbTpoDoc = document.getElementById("cmbtipodoc") as HTMLSelectElement; 
    const _txtNroDoc = document.getElementById("txtnrodoc") as HTMLInputElement;
    const _txtCodigoCli = document.getElementById("txtcodigo") as HTMLInputElement;
    let codProveedor: string;

    if(_cmbTpoDoc.value == "1"){
      codProveedor = "P000" + _txtNroDoc.value;
    }else{
      codProveedor = "P" + _txtNroDoc.value;
    }

    _txtCodigoCli.value = codProveedor;
  }

  capturarFile(event: any, tipo: string){
    const fileCopy = event.target.files[0];
    console.log(fileCopy);
    
    this.extraerBase64(fileCopy).then((file: any) => {
      // console.log(file);

      switch(tipo){
        case "1":
          this.archivo1 = (file.base).split(',')[1];
          this.nomArchivo1 = fileCopy.name;
          break;
        case "2":
          this.archivo2 = (file.base).split(',')[1];
          this.nomArchivo2 = fileCopy.name;
          break;
        case "3":
          this.archivo3 = (file.base).split(',')[1];
          this.nomArchivo3 = fileCopy.name;
          break;
        case "4":
          this.archivo4 = (file.base).split(',')[1];
          this.nomArchivo4 = fileCopy.name;
          break;
        case "5":
          this.archivo5 = (file.base).split(',')[1];
          this.nomArchivo5 = fileCopy.name;
          break;
        case "6":
          this.archivo6 = (file.base).split(',')[1];
          this.nomArchivo6 = fileCopy.name;
          break;
        case "7":
          this.archivo7 = (file.base).split(',')[1];
          this.nomArchivo7 = fileCopy.name;
          break;
        case "8":
          this.archivo8 = (file.base).split(',')[1];
          this.nomArchivo8 = fileCopy.name;
          break;
        case "9":
          this.archivo9 = (file.base).split(',')[1];
          this.nomArchivo9 = fileCopy.name;
          break;
        case "10":
          this.archivo10 = (file.base).split(',')[1];
          this.nomArchivo10 = fileCopy.name;
          break;
        case "11":
          this.archivoaux1 = (file.base).split(',')[1];
          break;
        case "12":
          this.archivoaux2 = (file.base).split(',')[1];
          break;
        case "13":
          this.archivoaux3 = (file.base).split(',')[1];
          break;
        case "14":
          this.archivoaux4 = (file.base).split(',')[1];
          break;
        case "15":
          this.archivoaux5 = (file.base).split(',')[1];
          break;
      }
      
      // console.log(this.nomArchivo, this.archivo);
      this.msgError = "";
      this.confirmDialog = false;
    })
  }

  extraerBase64 = async ( $event: any ) => new Promise(( resolve, reject) => {
    let database: any;
    try {
      const unsafeFile = window.URL.createObjectURL($event);
      console.log(unsafeFile);

      const file = this.sanitizer.bypassSecurityTrustUrl(unsafeFile);
      const reader = new FileReader();
      reader.readAsDataURL($event);

      database = reader.onload = () => {
        resolve({
          base: reader.result
        });
      };

      database = reader.onerror = () => {
        resolve({
          base: null
        });
      };
    } catch (error) {
      database = null
    }

    return database;
  })

  async guardarProveedor(){
    const txtcodigo = document.getElementById("txtcodigo") as HTMLInputElement;
    const txtrazonsocial = document.getElementById("txtrazonsocial") as HTMLInputElement;
    const cmbCondPago = document.getElementById("cmbCondPago") as HTMLSelectElement;
    const cmbtipodoc = document.getElementById("cmbtipodoc") as HTMLSelectElement;
    const txtnrodoc = document.getElementById("txtnrodoc") as HTMLInputElement;
    const txtcorreo = document.getElementById("txtcorreo") as HTMLInputElement;
    const txttelfcli = document.getElementById("txttelfcli") as HTMLInputElement;
    const txtpassword = document.getElementById("txtpassword") as HTMLInputElement;

    if(txtrazonsocial.value == ""){
      this.msgError = "La razón social es obligatorio" 
      this.confirmDialog = true;
      return;
    }

    if(cmbCondPago.value == ""){
      this.msgError = "La condición de pago es obligatorio" 
      this.confirmDialog = true;
      return;
    }

    if(cmbtipodoc.value == ""){
      this.msgError = "El tipo de documento es obligatorio" 
      this.confirmDialog = true;
      return;
    }

    console.log(cmbtipodoc.value, txtnrodoc.value.length);
    
    if(txtnrodoc.value == ""){
      this.msgError = "El número de documento es obligatorio" 
      this.confirmDialog = true;
      return;
    }else{
      if(cmbtipodoc.value == "1"){
        if(txtnrodoc.value.length != 8){
          this.msgError = "El número de documento tiene que ser de 8 dígitos" 
          this.confirmDialog = true;
          return;
        }
      }

      if(cmbtipodoc.value == "6"){
        if(txtnrodoc.value.length != 11){
          this.msgError = "El número de documento tiene que tener 11 dígitos" 
          this.confirmDialog = true;
          return;
        }
      }
    }

    if(txtcorreo.value == ""){
      this.msgError = "El correo es obligatorio" 
      this.confirmDialog = true;
      return;
    }

    if(txttelfcli.value == ""){
      this.msgError = "El teléfono es obligatorio" 
      this.confirmDialog = true;
      return;
    }
    
    if(txtpassword.value == ""){
      this.msgError = "La contraseña es obligatorio" 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo1 == "" || this.archivo1 == undefined || this.archivo1 == null){
      this.msgError = "Tiene que seleccionar el archivo Declaración Jurada Anticorrupción " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo2 == "" || this.archivo2 == undefined || this.archivo2 == null){
      this.msgError = "Tiene que seleccionar el archivo Código Ética y Conducta " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo3 == "" || this.archivo3 == undefined || this.archivo3 == null){
      this.msgError = "Tiene que seleccionar el archivo Declaración Jurada de Confidencialidad " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo4 == "" || this.archivo4 == undefined || this.archivo4 == null){
      this.msgError = "Tiene que seleccionar el archivo Declaración Jurada de Conocimiento " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo5 == "" || this.archivo5 == undefined || this.archivo5 == null){
      this.msgError = "Tiene que seleccionar el archivo Ficha RUC " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo6 == "" || this.archivo6 == undefined || this.archivo6 == null){
      this.msgError = "Tiene que seleccionar el archivo Reporte Tributario SUNAT " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo7 == "" || this.archivo7 == undefined || this.archivo7 == null){
      this.msgError = "Tiene que seleccionar el archivo Reporte Platadorma AL/FT " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo8 == "" || this.archivo8 == undefined || this.archivo8 == null){
      this.msgError = "Tiene que seleccionar el archivo Reporte Central Riesgo " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo9 == "" || this.archivo9 == undefined || this.archivo9 == null){
      this.msgError = "Tiene que seleccionar el archivo Reporte R03: Trabajadores " 
      this.confirmDialog = true;
      return;
    }

    if(this.archivo10 == "" || this.archivo10 == undefined || this.archivo10 == null){
      this.msgError = "Tiene que seleccionar el archivo DNI Representante  Legal " 
      this.confirmDialog = true;
      return;
    }
    
    if(this.listaContacto.length<1){
      this.msgError = "Tiene que agregar un contacto" 
      this.confirmDialog = true;
      return;
    }

    if(this.listaDireccion.length<1){
      this.msgError = "Tiene que agregar una dirección" 
      this.confirmDialog = true;
      return;
    }

    if(this.listaCuenta.length<1){
      this.msgError = "Tiene que agregar una cuenta" 
      this.confirmDialog = true;
      return;
    }else{
      let contador: number = 0;
      for(let i=0; i<this.listaCuenta.length; i++){
        if((this.listaCuenta[i].tipo == "A" && this.listaCuenta[i].esDetraccion == false) || (this.listaCuenta[i].tipo == "C" && this.listaCuenta[i].esDetraccion == false)){
          contador++;
        }
      }

      if(contador == 0){
        this.msgError = "Tiene que agregar al menos una cuenta que no sea de detracción" 
        this.confirmDialog = true;
        return;
      }
    }

    const data: any =
    {
      "cardCode": txtcodigo.value,
      "cardName": txtrazonsocial.value,
      "licTradNum": txtnrodoc.value,
      "phone1": txttelfcli.value,
      "password": txtpassword.value,
      "emailAddress": txtcorreo.value,
      "formaPago": cmbCondPago.value,
      "u_EXX_TIPODOCU": cmbtipodoc.value,
      "archivo1": this.archivo1,
      "archivo2": this.archivo2,
      "archivo3": this.archivo3,
      "archivo4": this.archivo4,
      "archivo5": this.archivo5,
      "archivo6": this.archivo6,
      "archivo7": this.archivo7,
      "archivo8": this.archivo8,
      "archivo9": this.archivo9,
      "archivo10": this.archivo10,
      "aux1": this.archivoaux1,
      "aux2": this.archivoaux2,
      "aux3": this.archivoaux3,
      "aux4": this.archivoaux4,
      "aux5": this.archivoaux5,
      "direcciones": this.listaDireccion,
      "contactos": this.listaContacto,
      "cuentasBancarias": this.listaCuenta
    }

    console.log(data);
    
    this.flagLoad = true;
    try {
      const dataRpta: any = await this.proveedorService.crearProveedor(data).toPromise();
     
      const xRpta: string = dataRpta.toString() || "";

      this.msgError = xRpta;
      if( this.msgError.includes("éxito") ){
        this.flagExito = true;
      }else{
        this.confirmDialog = true;
      }

    } catch (error: any) {
      this.msgError = error.toString();
      this.confirmDialog = true;
    }

    this.flagLoad = false;
  }

  btnOk(){
    this.router.navigateByUrl('/login', { replaceUrl: true });
  }

  salir(){
    this.router.navigateByUrl('/login', { replaceUrl: true });
  }

  async descargarFormato(){
    for(let i = 0; i<this.listaFormatos.length; i++){
      const name: string = this.listaFormatos[i].descripcion;
      await this.proveedorService.downloadFormato(name).toPromise().then(data => {
        const archivo: string = data || "";
        // this.pdfSrc = archivo;
        // console.log("archivo", this.pdfSrc);
        var byteCharacters = atob(archivo);
        var byteNumbers = new Array(byteCharacters.length);

        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers); 
  
        let filename = name;  
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
}

import { Component } from '@angular/core';
import { NgStyle } from '@angular/common';
import { IconDirective } from '@coreui/icons-angular';
import { ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, InputGroupComponent, InputGroupTextDirective, FormControlDirective, ButtonDirective } from '@coreui/angular';
import { Router } from '@angular/router';
import { SeguridadService } from 'src/app/services/seguridad.service';
import { ProveedorService } from 'src/app/services/proveedor.service';
import { AlertComponent } from '@coreui/angular';
import { CommonModule } from '@angular/common'; 
import { MaestroService } from 'src/app/services/maestro.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    standalone: true,
    imports: [AlertComponent, ContainerComponent, RowComponent, ColComponent, CardGroupComponent, TextColorDirective, CardComponent, CardBodyComponent, FormDirective, 
              InputGroupComponent, InputGroupTextDirective, IconDirective, FormControlDirective, ButtonDirective, NgStyle, CommonModule]
})
export class LoginComponent {

  errorValidacion: Boolean = false;
  confirmDialog: Boolean = false;
  flagExito: Boolean = false;
  flagLoad: Boolean = false;
  msgError: string = "";
  user: string = "";
  pass: string = "";
  
  constructor(private seguridadService: SeguridadService, private router: Router, private proveedorService: ProveedorService, private maestroService: MaestroService) {}

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  async validar(){
    const txtUser = document.getElementById("user") as HTMLInputElement;
    const txtPass = document.getElementById("pass") as HTMLInputElement;
    this.user = txtUser.value;
    this.pass = txtPass.value;

    if(this.user == "" || this.user == null){
      this.errorValidacion = true;
      this.msgError = "Tiene que escribir un usuario";
      return;
    }
    
    if(this.pass == "" || this.pass == null){
      this.errorValidacion = true;
      this.msgError = "Tiene que escribir una contraseña";
      return;
    }

    try {
      console.log("enviando datos...", this.user, this.pass);      
      const data: any = await this.seguridadService.validarAcceso(this.user, this.pass).toPromise();
      console.log("login", data)
      if(data.flag){     
        // localStorage.setItem("tb", data);
        sessionStorage.setItem("tb", JSON.stringify(data.token))

        this.maestroService.getConfiguracion().toPromise().then(dataConfig => {
          sessionStorage.setItem("config", JSON.stringify(dataConfig))
        });

        const datapro: any = await this.proveedorService.getCliente(this.user).toPromise();
        localStorage.setItem("prov", JSON.stringify(datapro));
        console.log("datapro", JSON.stringify(datapro));
        if(datapro.cardCode != null){
          sessionStorage.setItem("prov", JSON.stringify(datapro));
          this.router.navigateByUrl('/orden-compra', { replaceUrl: true });
        }
        else{
          sessionStorage.setItem("prov", "user");
          this.router.navigateByUrl('/proveedor-listar', { replaceUrl: true });
        }
      }else{
        console.log("error", data)
        
        console.log(data.flag, data.name)
        this.errorValidacion = true;
        this.msgError = data.name;
      }
    } catch (error) {
      console.log(error)
      this.errorValidacion = true;
      this.msgError = "Usuario o clave incorrecto";
    }    
  }

  keyPress(eventVal: any){
    // console.log(eventVal)
    this.errorValidacion = false;
    if (eventVal.charCode == 13){
      this.validar();
    }else{
      this.errorValidacion = false;
    }
  }

  registrarse(){
    this.router.navigateByUrl('/register', { replaceUrl: true });
  }

  async enviarOlvido(){
    const txtCorreo = document.getElementById("txtCorreo") as HTMLInputElement;
    if(txtCorreo.value == ""){
      this.flagExito = true;
      this.msgError = "Debe colocar su R.U.C"
      return;
    }

    this.confirmDialog=false;
    this.flagLoad = true;
    const random = Math.random().toString(36).substring(2,12)
    // console.log(random.substring(0, 5).toUpperCase())
    await this.seguridadService.sendEnvioCorreo(txtCorreo.value, random.substring(0, 5).toUpperCase()).toPromise().then(data => {
      this.msgError = data?.toString() || "";
      this.flagExito = true;
    })
    txtCorreo.value = "";
    this.flagLoad = false;
  }
}

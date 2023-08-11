import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AuthService } from './_services/auth.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RegisterComponent } from './API_Services/Identity/Identity/register/register.component';
import { LoginComponent } from './API_Services/Identity/Identity/login/login.component';
import { ItemsAlbumComponent } from './API_Services/Inventory/CatalogueItem/catalogue-items-album/catalogue-items-album.component';
import { HttpHeaderInterceptor } from './_interceptors/http.header.interceptor';
import { HttpImagePipe } from './_pipes/http.image.pipe';
import { AddCatalogueItemPopUpComponent } from './API_Services/Ordering/Cart/add-catalogue-item-pop-up/add-catalogue-item-pop-up.component';
// import {enableProdMode} from '@angular/core';


// enableProdMode();
@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    ItemsAlbumComponent,
    HttpImagePipe,
    AddCatalogueItemPopUpComponent
  ],
  //entryComponents: [AddCatalogueItemPopUpComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MatDialogModule,
    BrowserAnimationsModule
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    { provide: HTTP_INTERCEPTORS, useClass: HttpHeaderInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
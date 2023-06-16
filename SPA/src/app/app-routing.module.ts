import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ItemsAlbumComponent } from './items-album/items-album.component';

const routes: Routes = [
  {
    path: '',
    component: ItemsAlbumComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

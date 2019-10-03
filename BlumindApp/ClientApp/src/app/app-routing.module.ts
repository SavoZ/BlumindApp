import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { LoginComponent } from './login/login.component';
import { EditProductComponent } from './pages/products/edit-product/edit-product.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '', canActivate: [AuthGuard], component: NavMenuComponent, children: [
      {
        path: 'product', canActivate: [AuthGuard], children: [
          // { path: '', canActivate: [AuthGuard], component: UserIndexComponent },
          { path: 'add', canActivate: [AuthGuard], component: EditProductComponent },
          { path: 'edit/:id', canActivate: [AuthGuard], component: EditProductComponent }
        ]
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { LoginComponent } from './login/login.component';
import { EditProductComponent } from './pages/products/edit-product/edit-product.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { EditCategoryComponent } from './pages/categories/edit-category/edit-category.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '', canActivate: [AuthGuard], component: NavMenuComponent, children: [
      {
        path: 'product', canActivate: [AuthGuard], children: [
          { path: 'add', canActivate: [AuthGuard], component: EditProductComponent },
          { path: 'edit/:id', canActivate: [AuthGuard], component: EditProductComponent }
        ]
      },
      {
        path: 'category', canActivate: [AuthGuard], children: [
          { path: 'add', canActivate: [AuthGuard], component: EditCategoryComponent },
          { path: 'edit/:id', canActivate: [AuthGuard], component: EditCategoryComponent }
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

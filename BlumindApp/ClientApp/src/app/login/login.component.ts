import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  hide = true;
  loading = false;
  submitted = false;
  returnUrl: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthenticationService) {
    this.createForm();
  }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get f() {
    return this.loginForm.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) { return; }

    this.loading = true;
    try {
      this.authService.login(this.f.username.value, this.f.password.value, this.returnUrl);
    } catch (error) {
      alert(error);
    }
  }

  private createForm() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

}

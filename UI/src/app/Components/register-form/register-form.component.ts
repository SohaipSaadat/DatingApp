import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountsService } from 'src/app/Services/accounts.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css'],
})
export class RegisterFormComponent implements OnInit {
  @Output() canselRegister = new EventEmitter<boolean>();
  validationError: string[] = [];
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  constructor(
    private accountService: AccountsService,
    private fb: FormBuilder,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.initializeFrom();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeFrom() {
    this.registerForm = this.fb.group({
      gender: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      knownAs: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      interests: ['', [Validators.required]],
      lookingFor: ['', [Validators.required]],
      introduction: ['', [Validators.required]],
      password: [
        '',
        [Validators.required, Validators.minLength(3), Validators.maxLength(8)],
      ],
      confirmPassword: ['', [Validators.required, this.matchValue('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () =>
        this.registerForm.controls['confirmPassword'].updateValueAndValidity(),
    });
  }
  register() {
    const date = this.getDateOnly(
      this.registerForm.controls['dateOfBirth'].value
    );
    const value = { ...this.registerForm.value, dateOfBirth: date };
    this.accountService.register(value).subscribe({
      next: () => this.router.navigateByUrl('/members'),
      error: (error) => {this.validationError = error; console.log(error) },
    });
  }

  cansel() {
    this.canselRegister.emit(false);
  }
  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value
        ? null
        : { noMatching: true };
    };
  }
  getDateOnly(dob: string | undefined) {
    if (!dob) return;
    const date = new Date(dob);
    return new Date(
      date.setMinutes(date.getMinutes() - date.getTimezoneOffset())
    )
      .toISOString()
      .slice(0, 10);
  }
}

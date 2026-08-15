import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export function AutoCompleteComplexObject(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    // Must be an object
    if (!value || typeof value !== 'object') {
      return { invalidObject: true };
    }

    return null;
  };
}

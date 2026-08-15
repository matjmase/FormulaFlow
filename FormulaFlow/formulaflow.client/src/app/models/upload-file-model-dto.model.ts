export interface UploadFileModelDto {
  skipHeader: boolean;
  dateColumnIndex: number;
  valueColumnIndex: number;
  collisionBehavior: UploadFileModelDtoCollisionBehavior;
}

export enum UploadFileModelDtoCollisionBehavior {
  SkipExisting = 0,
  OverwriteExisting = 1,
  CreateNewEntry = 2,
}

export interface Message {
  id: number;
  senderId: number;
  senderUserName: string;
  senderPhotoUrl: string;
  reciverId: number;
  reciverUserName: string;
  reciverPhotoUrl: string;
  content: string;
  dateRead: Date;
  dateSent: Date;
}

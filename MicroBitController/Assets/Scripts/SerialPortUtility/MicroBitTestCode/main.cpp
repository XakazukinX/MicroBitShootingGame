#include "MicroBit.h"
Serial pc(USBTX, USBRX);

int main()
{
    //ボーレートを設定
    pc.baud(57600);
    while(1) 
    {
        wait(0.5);
        /*読み込む内容があるときにReceiveを送信する*/
        while(pc.readable() == 1)
        {
            pc.getc();
            pc.printf("Receive\n");
        }
    }
}
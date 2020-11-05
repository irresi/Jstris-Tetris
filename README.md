# Jstris-Tetris


테트리스 하기
https://jstris.jezevec10.com/
- - -
```
1. 크롬으로 여세요
2. 북마크를 쓰지 마세요
3. 텍스트 및 앱 크기 : 100%
4. 디스플레이 해상도 : 1920*1080
```


<img src="image/jstris 예시 화면.png"
     alt="Markdown Monster icon"
     style="float: left; margin-right: 10px;" />
     
<img src="image/화면 설정.png"
     alt="Markdown Monster icon"
     style="float: left; margin-right: 10px;" />
     
https://jstris.jezevec10.com/
- - -
# 부연 설명

GeneticAlgorithm.cs에서 먼저 학습을 한다. 
학습을 할 때 Weight 클래스에서 함수들의 반환값에 각각의 개체에 있는 다른 계수를 곱하여 가중치를 구하고 가중치가 가장 높은 곳에 블록을 놓게 된다.
Weight 클래스에서 클리어 줄 수, 공격할 수 있는 줄 수, 최대 높이, 블록 밑 빈칸의 수, 사방이 막혀있는 블록 수 등을 구할 수 있는 함수가 있다.

# PVP에서 과도하게 악용하거나 Jstris 사이트에 로그인하고 플레이하지 마세요

로그인하고 돌리면 리더보드에 1위로 등록됩니다.
웹사이트 운영에 피해를 줄 수 있으니 로그인하지 않고 돌려보세요.

# 클래스별 설명

GeneticAlgorithm.cs는 핵심적인 클래스로 유전 알고리즘을 통해 가중치 함수별 계수를 구함. Getpixel.cs는 화면에서 특정 픽셀을 읽게 도움. 
Instruction.cs는 어떤 프로그램인지 소개
Map.cs는 화면으로부터 맵 정보를 읽고 테트리스를 클리어하는 등 맵의 상태에 관한 정보
MoveBlock.cs는 블록을 움직일 수 있는 지 여부
Program.cs는 처음 프로그램이 작동하는 곳으로 학습을 할지 아니면 직접 플레이할지 설명볼지 결정
Solve.cs는 핵심 클래스로 jstris에서 직접 테트리스를 플레이할 수 있게 함. 
TBlock.cs는 Block.cs와 유사하지만 오프라인 학습용.
Weight.cs는 가중치를 반환하는 함수, TWeight.cs는 Weight의 오프라인 학습용

# 실행 영상
https://serviceapi.nmv.naver.com/flash/convertIframeTag.nhn?vid=EF8F1569AD6E603A8646D2A0111826C28D30&outKey=V1279c7b3503a2363f28085f379e46c1a1acc63d8c211f4b61c5f85f379e46c1a1acc 

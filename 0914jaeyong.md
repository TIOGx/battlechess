## 0914

### 추가 내역

- [x] Issue#6에 따라 카메라 위치 변경 애니메이션 추가
- [x] Tag UI, Map 추가

### 버그 Fix or 수정 내역

- [x] 카메라 코루틴을 전면 애니메이션으로 교체
- [x] 턴 종료시 카메라가 정상 자리를 찾아가게끔 코드 수정
- [x] GameManager.cs의 코드 내 함수 오타 수정 & 레퍼런스 오타 수정
- [x] Build나 Move를 진행하다 취소한것에 대한 Canvas Active False 진행
- [] Background 필요 (Background Tag를 이용해서 object 컨트롤 요구됨)
- [] Build시 취소가 안되는 것 (Ray로 hit tag가 Background일 때 초기화를 생각 중에 있음)

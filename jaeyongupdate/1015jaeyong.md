# 2021 10 15 jaeyong

마무리단계라 수정한 버그가 많습니다.

## bug fix

1. 현재 코루틴을 이용해서 Choose the Position (타일을 선택하지 않고 빌드버튼 등), lack money(돈 부족하여 생성 불가) UI를 띄우고 있는데 사라지지 않는 버그를 수정하며, 코드 가독성을 높였습니다.
> OnclickMove (Move Button Onclick 이벤트) 함수에도 코루틴을 설정시켰습니다.
> 코루틴문도 수정하였습니다. 확인 부탁드립니다.

2. 킹이 움직일 떄 애니메이션 반영이 올바르지 못했던 것을 수정하였습니다.
> 이전부터 효원님이 얘기하셨던 애니메이터 로직 부분에 오류가 있었다고 판단하여, 로직을 수정했습니다.

3. 말을 다시 누르면 이동취소가 가능하게 수정하였습니다.
> MoveManager의 가독성을 높이기 위해 함께 사용되는 함수들을 InitilizeSetting이라는 함수로 따로 묶어 사용하였습니다.

4. 자신의 턴이 아닌데도 자신의 말이 바라보는 dir를 수정할 수 있던 오류를 수정하였습니다.
> MoveManager의 턴 확인 로직을 추가하였고 메소드는 누구나 알아볼 수 있게 제작하였습니다.

5. 상태창 Type에 네글자 이상 반영되게 수정했습니다.

6. 킹의 위치를 살짝 변경했습니다.

<hr>

## testing


## needs

1. 애니메이션 타일을 누르는 경우에는 게임에 지장이 없다고 판단하여 수정이 필요하지 않을것이라고 예상되어 변경하진 않았습니다.
> 수정이 필요하다면, 로직은 타일이 아닌 애니메이션 오브젝트 태그와 레이가 충돌했을 때 부모의 오브젝트 트랜스폼으로 위치를 잡아주면 수정은 가능합니다.

<hr>

**Pull 한 후, 일어날 수 있다고 판단되거나 시뮬레이션을 진행하였을 때 나타나는 버그에 대해 제보해주세요.**

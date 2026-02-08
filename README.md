# 🛡️⚔️ Sword & Shield

KAIST 몰입캠프 2025 Winter · **Week 3**  
개발 기간: **2026.01.22 ~ 2026.01.28**  
장르: **로컬 2인 협동 · 탄막 피하기 · 보스전**

> 칼과 방패가 자기들을 버리고 **마법사로 전직한 전사**에게 복수하러 떠난다.  
> 탑을 지키는 보스들을 쓰러트리고 최상층에서 마법사를 처치하라!

---

## 🎮 게임 소개

**Sword & Shield**는 두 플레이어가 각각 **방패** 와 **칼** 역할을 맡아  
탄막 패턴을 피하고, 협력해서 보스를 격파하며 **3스테이지 클리어**를 목표로 하는 로컬 멀티 협동 게임입니다.

- **승리 조건**: 3 스테이지 보스 클리어
- **패배 조건**: 두 플레이어가 공유하는 **공동 체력**이 0이 되면 게임 오버

---

## 🕹️ 조작법

| Player | 역할 | 이동 | 행동 |
|---|---|---|---|
| Player 1 | 🛡️ 방패 | `W A S D` | (현재 기능 없음) |
| Player 2 | ⚔️ 칼 | `Arrow Keys` | `Right Shift` 공격 |

---

## ✨ 구현 포인트 (Features)

### 🧠 보스 AI / 패턴
- **Coroutine 기반 Boss State Machine**
- 여러 형태의 탄막 패턴 생성
  - Shockwave / Homing Missile / Frenzy / Sword / Circle 등 (구현된 상태 기준으로 업데이트)

### 🎥 연출
- **URP(Universal Render Pipeline) 기반 Post-processing**
- 카메라 연출 (Camera shake, Hitstop)

---

## 🛠️ 기술 스택

- **Unity 2022 (2D)**
- **C#**
- Coroutine 기반 패턴/상태 로직
- **URP Post-processing**


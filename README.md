
<p align="center">
  <img src="Docs/PFP%20Logo.png" alt="Planetfall Protocol Logo" width="1000">
</p>

**Planetfall Protocol** is a single-player, narrative-driven sci-fi exploration game developed in Unity. Players assume the role of **Atlas**, an interplanetary survey pilot who crash-lands on the alien world of **Aurelia** after a mission gone wrong. Stranded in an unfamiliar environment and cut off from home, the player must explore the ruins of a fallen civilization, gather resources, interact with survivors, and work toward repairing their ship.

At the center of the experience is **VERA** (*Vital Environmental Resource Analyzer*), Atlas’s AI companion. VERA assists with navigation, environmental interpretation, translation, and contextual guidance as the player uncovers the truth behind Aurelia’s collapse. As exploration progresses, players are confronted with ethical choices that shape both the story and the fate of the world around them.

---

## Table of Contents

- [Overview](#overview)
- [Narrative Premise](#narrative-premise)
- [Core Gameplay](#core-gameplay)
- [Key Features](#key-features)
- [World and Setting](#world-and-setting)
- [Player Progression](#player-progression)
- [Current Project Scope](#current-project-scope)
- [Technical Design](#technical-design)
- [Controls](#controls)
- [Target Audience](#target-audience)
- [Inspirations](#inspirations)
- [Platform](#platform)
- [Monetization](#monetization)
- [Development Team](#development-team)
- [Project Status](#project-status)
- [Links](#links)
- [Assets](#assets)
- [License](#license)

---

## Overview

Planetfall Protocol is designed as an atmospheric exploration experience that blends narrative discovery, world-building, interaction systems, and moral decision-making. Rather than emphasizing fast-paced combat, the game focuses on curiosity, immersion, and consequence. Players learn about Aurelia through environmental storytelling, NPC conversations, translated fragments of lost knowledge, and VERA’s assistance.

The project aims to balance strong narrative identity with technically grounded gameplay systems, including first-person movement, interactive dialogue, scrap collection, health and hazard mechanics, save/load functionality, and responsive UI systems.

---

## Narrative Premise

Atlas begins the game on a routine deep-space survey mission before unexpectedly crash-landing on **Aurelia**, a world marked by silence, scattered ruins, and fractured survivor communities. While attempting to repair the ship and escape, Atlas gradually discovers that Aurelia was once home to advanced civilizations powered by a planetary machine tied to the planet’s core. When that system failed, technology across the world collapsed, leaving behind isolated colonies, broken infrastructure, and lost knowledge.

As the player explores, it becomes clear that Aurelia’s downfall is tied not only to its own history, but also to uncomfortable truths about humanity’s role in worlds like it. Meanwhile, VERA, initially a trusted guide, begins to exhibit glitches, memory gaps, and suspicious inconsistencies. What first appears to be malfunction slowly becomes part of the game’s deeper moral and narrative conflict.

Ultimately, the player must decide whether to:
- repair the ship and leave Aurelia behind, or
- remain and help rebuild what was lost.

---

## Core Gameplay

Planetfall Protocol centers on a narrative exploration loop:

1. **Explore** unfamiliar environments and discover areas of interest.
2. **Interact** with NPCs to gain story context, progression, and translation insight.
3. **Scavenge** scrap and other resources from the world.
4. **Consult VERA** for guidance, contextual prompts, and dialogue.
5. **Survive hazards** through health management and environmental awareness.
6. **Repair the ship** by collecting enough scrap to unlock the ending sequence.
7. **Make meaningful choices** that shape the player’s interpretation of the world and its future.

The intended player experience is reflective, immersive, and self-directed. The game is designed so that mechanics are introduced organically through play rather than through overly disruptive tutorials.

---

## Key Features

### Narrative-Driven Exploration
Players uncover Aurelia’s history through exploration, ruins, environmental storytelling, and character interaction rather than through exposition alone.

### AI Companion: VERA
VERA serves as Atlas’s primary guide and one of the game’s defining features. She supports the player through:
- contextual guidance,
- environmental analysis,
- translation assistance,
- dialogue interaction,
- adaptive hints when the player is inactive or uncertain.

VERA is a character with narrative weight, ambiguity, and evolving significance.

### NPC Dialogue and Translation
NPC interactions provide story progression, world context, and translation advancement. As players engage with new characters and discoveries, they gain a deeper understanding of the planet’s inhabitants, relationships, and history.

### Scrap Collection and Ship Repair
Scavenging is a core gameplay mechanic. Players collect scrap via interacting with objects in the environment, track it through the HUD, and use it to work toward repairing their ship and advancing the story.

### Environmental Hazard and Health Systems
The world includes environmental dangers that damage the player. Health is clearly communicated through the HUD and regenerates when the player is no longer in danger. Fall damage also reinforces environmental awareness and traversal consequences.

### Moral Choice and Narrative Outcome
Planetfall Protocol is built around player-driven decision-making. The player’s relationships, discoveries, and values ultimately influence how they respond to Aurelia’s future.

---

## World and Setting

Planetfall Protocol takes place on **Aurelia**, a once-advanced alien world now reduced to scattered colonies and silent machinery.

### Backstory
Aurelia was formerly powered by advanced technology tied to a complex planetary machine that regulated energy drawn from the planet’s core. When that system catastrophically failed, technology across the planet collapsed. Over time, knowledge of the machine, its failure, and the civilization that built it faded into myth and fragments.

Now, small communities survive among ruins, trying to rebuild while interpreting the remnants of a world they no longer fully understand.

### Major Regions
The game’s world and narrative identity are built around three major regions:

- **Echo Basin**  
  The crash site and tutorial-oriented opening region. Mysterious, and alive with first-contact tension.

- **Virelia Frontier**  
  A more hopeful and expansive area that reflects rebuilding, possibility, and connection.

- **The Riftlands**  
  A cold, vast, ruin-filled region with an ancient, solemn atmosphere.

These areas reinforce the emotional arc of the experience through environmental design, atmosphere, and sound identity.

---

## Player Progression

The player begins with very limited context:
- Atlas is a survey pilot.
- VERA is their AI expedition companion.
- The intended mission involved travel to **Aphelion**.
- The crash on Aurelia was unexpected.
- The reason for Aurelia’s current condition is unknown.

Progression unfolds through:
- exploration of the world,
- first-time NPC interactions,
- translation progress,
- scrap collection,
- environmental discovery,
- narrative choices,
- unlocking ship repair and endgame decisions.

The player’s main short-term goal is survival and recovery. The long-term goal becomes much more complicated as Aurelia’s history, humanity’s involvement, and VERA’s internal conflict come into focus.

---

## Current Project Scope

The project’s current implementation scope has been refined to emphasize polish, clarity, and a stable gameplay loop.

### Included Core Systems
- First-person player movement with keyboard and mouse
- Scrap collection through interactable objects
- HUD displaying health and scrap
- NPC interaction through dialogue
- VERA interaction through a dedicated interface
- Pause menu and startup menu systems
- Save/load functionality using persistent data
- Environmental damage and health regeneration
- Fall damage
- On-screen interaction prompts
- Ship repair progression and endgame trigger
- UI scaling across common resolutions
- Audio controls and settings persistence

### Scope Adjustments
To better match development timeline and implementation quality goals, the project was streamlined from an earlier broader structure into a more testable and reliable experience. This included simplifying certain systems, reducing HUD complexity to essential information, and prioritizing persistence, interaction clarity, and core progression.

---

## Technical Design

### Engine and Languages
- **Unity (Version 6000.2.6f2) using the URP Pipeline**
- **C#**
- **Python**

### Core Technical Ideas
- Modular gameplay systems
- JSON-based save/load persistence
- Ink-based dialogue integration
- UI systems for prompts, dialogue, HUD, and menus
- Reusable prefabs and interaction-driven logic
- Performance-oriented design targets for stable play

### System Highlights
- **Save/Load:** persists essential gameplay state such as player position, scrap count, quest or narrative flags, and related progress data.
- **HUD:** continuously displays core gameplay information and updates in response to collection, damage, and healing.
- **Interaction System:** provides clear prompt-driven interaction for NPCs, scrap, ship repair, and VERA.
- **Dialogue Architecture:** supports branching conversations and narrative progression.
- **Adaptive Guidance:** allows VERA to provide contextual player support.

---

## Controls

> Final bindings may continue to evolve during development.

> The bindings listed below are default bindings, and can be changed in the game.

| Action                            | Default Control    |
| --------------------------------- | ------------------ |
| Move                              | `W`, `A`, `S`, `D` |
| Look Around                       | Mouse              |
| Interact with NPCs / Scrap / Ship | `E`                |
| Open / Close VERA Interface       | `Q`                |
| Open Pause Menu                   | `Esc`              |

---

## Target Audience

Planetfall Protocol is intended for players aged **12+ / 13+** who enjoy:
- atmospheric exploration,
- narrative-driven experiences,
- world-building,
- ethical and emotional storytelling,
- light survival and interaction systems,
- slower-paced, self-directed gameplay.

The game is designed to be accessible to a wide range of players by emphasizing intuitive controls, readable interfaces, and organically introduced mechanics.

---

## Inspirations

Planetfall Protocol draws inspiration from games such as:

- **Subnautica** – for exploration and discovery
- **Outer Wilds** – for mystery-driven world design
- **No Man’s Sky** – for tone and sci-fi atmosphere
- **Firewatch** – for first-person narrative and dialogue-driven immersion

While inspired by these titles, Planetfall Protocol distinguishes itself through its focus on:
- translation and communication,
- VERA’s semi-trustworthy AI presence,
- morality-driven outcomes,
- a more explicitly reflective and ethical narrative structure.

---

## Platform

**Target Platform:** Windows PC

---

## Monetization

TBA (Coming Soon!)

---

## Development Team

**University of Nevada, Reno**  
Department of Computer Science & Engineering

CS425 - Software Engineering (Fall 2025)

CS426 - Senior Projects in Computer Science (Spring 2026)

**Team 23**
- Bella Picasso-Kennedy
- Edgar Lopez
- Emma Cornia
- Rajat Sharma

**Instructors**
- Dr. Dave Feil-Seifer
- Vinh Le

**Project Advisors**
- Joshua Dahl
- Jack Ratermann
- Dr. Emily Hand
- Dr. Ankita Shukla

---

## Project Status

Planetfall Protocol is currently in active development.

The overall design direction is established, and the project has focused on building a reliable core gameplay experience around:
- exploration,
- interaction,
- dialogue,
- resource collection,
- UI responsiveness,
- persistence systems,
- and narrative progression.

Recent work has emphasized improving gameplay readability, world-building, menu and HUD polish, VERA responsiveness, sound design, movement quality, and progression stability.

---

## Summary

Planetfall Protocol is a sci-fi exploration game about ruin, recovery, truth, and choice. Through atmospheric world-building, guided interaction, resource gathering, and moral consequence, the project aims to deliver a player experience that is both technically grounded and narratively meaningful.

As Atlas, players are trying to escape a broken world, but are also deciding what responsibility they carry once they understand how it broke.

---

## Links

### Core Documentation
- [Game Design Document (GDD)](https://docs.google.com/document/d/1ozdL9RmAy_N4gYzJj2tq_Gek3nOLMQ20TVtlnPTyB2Q/edit?usp=sharing)

### CS 425 Deliverables
- [PA 1](https://docs.google.com/document/d/1DEWzpzZcpaAqY1eQg3ZApDzeCNH5C6B7vjjKMtDbels/edit?usp=sharing)
- [PA 2](https://docs.google.com/document/d/1OYw-JEJIVd5cJyZPckCvyMZSC_Zm5NidqH_eR6BM-9M/edit?usp=sharing)
- [PA 3](https://docs.google.com/document/d/1d0ejROuFziJD5Z4kRDmTg1QuBgVGcrGbXDNm9mRjyBc/edit?usp=sharing)
- [PA 4](https://docs.google.com/document/d/1vIXq9cwnZdau6s4HAyTIq9P7XBg8BEoeP3upjpw_C5M/edit?usp=sharing)

### CS 426 Deliverables
- [PA 1](https://docs.google.com/document/d/1ssKvokaepq2t0_u_zuV-nd8FAIqHHMnjpy-qedFBsAI/edit?usp=sharing)
- [PA 2](https://docs.google.com/document/d/1auC9gQmMFeDhm_uzgr4fXdaWbEAnWNQlT3yc5r0oqtU/edit?usp=sharing)
- [PA 3](https://docs.google.com/document/d/1Z_i0wyHHVj86owRK1KAev2nezGfUc6br_Kc3PnZZPb8/edit?usp=sharing)
- [PA 4](https://docs.google.com/document/d/1K7U9MnyzgV5u9DmL1bDzBNJ74owPcbEiEw4du-6Qqxo/edit?usp=sharing)

---

## Assets

TBA (Coming Soon!)

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

Note: Third-party assets, packages, audio, models, fonts, and other external resources remain subject to their own respective licenses and are not automatically covered by the MIT License.